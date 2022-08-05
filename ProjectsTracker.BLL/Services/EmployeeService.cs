using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.Infrastructure;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTracker.DAL.Interfaces;
using ProjectsTracker.DAL.Repository;
using ProjectsTracker.DAL.Models;

namespace ProjectsTracker.BLL.Services {
    public class EmployeeService : IEmployeeService {
        IUnitOfWork Database { get; set; }
        public EmployeeService(string connectionString) {
            Database = new EFUnitOfWork(connectionString);
        }

        public async Task<EmployeeDTO> GetEmployeeAsync(int? id) {
            var employee = await Database.Employees.Get(id.Value);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>()).CreateMapper();
            return mapper.Map<EmployeeDTO>(employee);
        }

        public IEnumerable<EmployeeDTO> GetEmployees() {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Employee>, List<EmployeeDTO>>(Database.Employees.GetAll());
        }


        public async Task AddOrEditEmployeeAsync(EmployeeDTO employee) {
            if (employee == null)
                throw new ValidationException("employee = null", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeDTO, Employee>()).CreateMapper();
            var oldEmployee = await Database.Employees.Get(employee.Id);
            if (oldEmployee == null)
                Database.Employees.Create(mapper.Map<EmployeeDTO, Employee>(employee));
            else
                mapper.Map(employee, oldEmployee);
            Database.Save();
        }
        public async Task DeleteEmployeeAsync(int? id) {
            var employee = await Database.Employees.Get(id.Value);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            Database.Employees.Delete(id.Value);
            Database.Save();
        }

        public async Task TieEmployeeProjectAsync(int? projectId, int? employeeId) {
            var project = await Database.Projects.Get(projectId.Value);
            if (project == null)
                throw new ValidationException("project is not found", "");

            var employee = await Database.Employees.Get(employeeId.Value);
            if (employee == null)
                throw new ValidationException("employee is not found", "");
            project.Employees?.Add(employee);
            Database.Save();
        }        
        
        public async Task SeparateEmployeeProjectAsync(int? projectId, int? employeeId) {
            var project = await Database.Projects.Get(projectId.Value);
            if (project == null)
                throw new ValidationException("project not found", "");

            var employee = await Database.Employees.Get(employeeId.Value);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            if (project.TeamLead?.Id == employee.Id)
                project.TeamLead = null;
            project.Employees?.Remove(employee);
            Database.Save();
        }

    }
}
