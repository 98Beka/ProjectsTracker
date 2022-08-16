using AutoMapper;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.Infrastructure;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTracker.DAL.Interfaces;
using ProjectsTracker.DAL.Repository;
using ProjectsTracker.DAL.Models;
using ProjectsTracker.BLL.Profiles;

namespace ProjectsTracker.BLL.Services {
    public class ProjectService : IProjectService {
        readonly private IUnitOfWork _database;
        readonly IMapper _mapper;
        public ProjectService(string connectionString) {
            _database = new EFUnitOfWork(connectionString);

            _mapper = new MapperConfiguration( c => {
                c.AddProfile<ProjectMapperConfig>();
                c.AddProfile<EmployeeMapperConfig>();
                }
            ).CreateMapper();
        }

        public async Task<ProjectDTO> GetProjectAsync(int id) {
            Project project = await _database.Projects.Get(id);
            if (project == null)
                throw new ValidationException("project not found", "");
            var mapper = _mapper;
            return mapper.Map<ProjectDTO>(project);
        }
        public IEnumerable<ProjectDTO> GetProjects(IProjectFilter filter) {
            List<ProjectDTO> output = _mapper.Map<List<ProjectDTO>>(_database.Projects.GetAll());
            return filter.Filtrate(output);
        }
        public async Task<ProjectDTO> AddOrEditProjectAsync(ProjectDTO project) {
            if (project == null)
                throw new ValidationException("project = null", "");
            var oldProject = await _database.Projects.Get(project.Id);
            if (oldProject == null) {
                var res = await _database.Projects.Create(_mapper.Map<Project>(project));
                _database.Save();
                return (_mapper.Map<ProjectDTO>(res));
            }
            _mapper.Map(project, oldProject);

            _database.Save();
            return (project);
        }
        public async Task DeleteProjectAsync(int id) {
            Project project = await _database.Projects.Get(id);
            if (project == null)
                throw new ValidationException("project not found", "");
            _database.Projects.Delete(id);
            _database.Save();
        }
        public async Task AppointTeamleadAsync(int projectId, int employeeId) {
            var project = await _database.Projects.Get(projectId);
            if (project == null)
                throw new ValidationException("project not found", "");
            
            var employee = await _database.Employees.Get(employeeId);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            project.TeamLead = employee;
            _database.Save();
        }

        public async Task TieEmployeeProjectAsync(int projectId, int employeeId) {
            var project = await _database.Projects.Get(projectId);
            if (project == null)
                throw new ValidationException("project is not found", "");

            var employee = await _database.Employees.Get(employeeId);
            if (employee == null)
                throw new ValidationException("employee is not found", "");
            if(project.Employees?.FirstOrDefault(e => e.Id == employeeId) == null)
                project.Employees?.Add(employee);
            _database.Save();
        }        
        
        public async Task SeparateEmployeeProjectAsync(int projectId, int employeeId) {
            var project = await _database.Projects.Get(projectId);
            if (project == null)
                throw new ValidationException("project not found", "");

            var employee = await _database.Employees.Get(employeeId);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            if (project.TeamLead?.Id == employee.Id)
                project.TeamLead = null;
            project.Employees?.Remove(employee);
            _database.Save();
        }

    }
}
