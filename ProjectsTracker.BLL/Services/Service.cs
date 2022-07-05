using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.Infrastructure;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTracker.DAL.Interfaces;
using ProjectsTracker.DAL.Repository;
using ProjectsTracker.DAL.Models;

namespace ProjectsTracker.BLL.Services {
    public class Service : IService {
        IUnitOfWork Database { get; set; }
        public Service(string connectionString) {
            Database = new EFUnitOfWork(connectionString);
        }

        #region Project business logic
        public async Task<ProjectDTO> GetProjectAsync(int? id) {
            Project project = await Database.Projects.Get(id.Value);
            if (project == null)
                throw new ValidationException("project not found", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Project, ProjectDTO>()).CreateMapper();
            return mapper.Map<ProjectDTO>(project);
        }
        public IEnumerable<ProjectDTO> GetProjects(IProjectFilter filter) {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Project, ProjectDTO>()).CreateMapper();
            List<ProjectDTO> output = mapper.Map<IEnumerable<Project>, List<ProjectDTO>>(Database.Projects.GetAll());
            return filter.Filtrate(output);
        }
        public async Task AddOrEditProjectAsync(ProjectDTO project) {
            if (project == null)
                throw new ValidationException("project = null", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProjectDTO, Project>()).CreateMapper();
            var oldProject = await Database.Projects.Get(project.Id);
            if (oldProject == null)
                await Database.Projects.Create(mapper.Map<ProjectDTO, Project>(project));
            else
                mapper.Map(project, oldProject);
            Database.Save();
        }
        public async Task DeleteProjectAsync(int? id) {
            Project project = await Database.Projects.Get(id.Value);
            if (project == null)
                throw new ValidationException("project not found", "");
            Database.Projects.Delete(id.Value);
            Database.Save();
        }
        public async Task AppointTeamleadAsync(int? projectId, int? employeeId) {
            var project = await Database.Projects.Get(projectId.Value);
            if (project == null)
                throw new ValidationException("project not found", "");
            
            var employee = await Database.Employees.Get(employeeId.Value);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            project.TeamLead = employee;
            Database.Save();
        }

        #endregion

        #region Employee business logic
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

        #endregion

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
