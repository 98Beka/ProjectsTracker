using AutoMapper;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.Infrastructure;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTracker.DAL.Interfaces;
using ProjectsTracker.DAL.Repository;
using ProjectsTracker.DAL.Models;
using ProjectsTracker.BLL.Profiles;

namespace ProjectsTracker.BLL.Services {
    public class EmployeeService : IEmployeeService {
        readonly private IUnitOfWork _databes;
        readonly private IMapper _mapper;
        public EmployeeService(string connectionString) {
            _databes = new EFUnitOfWork(connectionString);
            _mapper = new MapperConfiguration(c => {
                c.AddProfile<ProjectMapperConfig>();
                c.AddProfile<EmployeeMapperConfig>();
            }
            ).CreateMapper();
        }

        public async Task<EmployeeDTO> GetEmployeeAsync(int id) {
            var employee = await _databes.Employees.Get(id);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public IEnumerable<EmployeeDTO> GetEmployees() {
            return _mapper.Map<IEnumerable<Employee>, List<EmployeeDTO>>(_databes.Employees.GetAll());
        }


        public async Task<EmployeeDTO> AddOrEditEmployeeAsync(EmployeeDTO employee) {
            if (employee == null)
                throw new ValidationException("employee = null", "");
            var oldEmployee = await _databes.Employees.Get(employee.Id);
            if (oldEmployee == null) {
                var res = await _databes.Employees.Create(_mapper.Map<Employee>(employee));
                _databes.Save();
                return (_mapper.Map<EmployeeDTO>(res));
            }
            _mapper.Map(employee, oldEmployee);
            _databes.Save();
            return (employee);
        }
        public async Task DeleteEmployeeAsync(int id) {
            var employee = await _databes.Employees.Get(id);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            _databes.Employees.Delete(id);
            _databes.Save();
        }

        public async Task TieEmployeeProjectAsync(int projectId, int employeeId) {
            var project = await _databes.Projects.Get(projectId);
            if (project == null)
                throw new ValidationException("project is not found", "");

            var employee = await _databes.Employees.Get(employeeId);
            if (employee == null)
                throw new ValidationException("employee is not found", "");
            project.Employees?.Add(employee);
            _databes.Save();
        }        
        
        public async Task SeparateEmployeeProjectAsync(int projectId, int employeeId) {
            var project = await _databes.Projects.Get(projectId);
            if (project == null)
                throw new ValidationException("project not found", "");

            var employee = await _databes.Employees.Get(employeeId);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            if (project.TeamLead?.Id == employee.Id)
                project.TeamLead = null;
            project.Employees?.Remove(employee);
            _databes.Save();
        }

    }
}
