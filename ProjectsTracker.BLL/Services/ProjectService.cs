﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.Infrastructure;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTracker.DAL.Interfaces;
using ProjectsTracker.DAL.Repository;
using ProjectsTracker.DAL.Models;

namespace ProjectsTracker.BLL.Services {
    public class ProjectService : IProjectService {
        readonly private IUnitOfWork _database;
        readonly IMapper _mapperToDTO;
        readonly IMapper _mapperTo;
        public ProjectService(string connectionString) {
            _database = new EFUnitOfWork(connectionString);
            _mapperToDTO = new MapperConfiguration(cfg => cfg.CreateMap<Project, ProjectDTO>()).CreateMapper();
            _mapperTo = new MapperConfiguration(cfg => cfg.CreateMap<ProjectDTO, Project>()).CreateMapper();
        }

        public async Task<ProjectDTO> GetProjectAsync(int? id) {
            Project project = await _database.Projects.Get(id.Value);
            if (project == null)
                throw new ValidationException("project not found", "");
            var mapper = _mapperToDTO;
            return mapper.Map<ProjectDTO>(project);
        }
        public IEnumerable<ProjectDTO> GetProjects(IProjectFilter filter) {
            List<ProjectDTO> output = _mapperToDTO.Map<IEnumerable<Project>, List<ProjectDTO>>(_database.Projects.GetAll());
            return filter.Filtrate(output);
        }
        public async Task AddOrEditProjectAsync(ProjectDTO project) {
            if (project == null || project.Id == 0)
                throw new ValidationException("project = null", "");
            Console.WriteLine(project.Id);
            var oldProject = await _database.Projects.Get(project.Id);
            if (oldProject == null) {
                await _database.Projects.Create(_mapperTo.Map<Project>(project));
            } else
                Console.WriteLine(oldProject.Id);
                _mapperTo.Map(project, oldProject);
            _database.Save();
        }
        public async Task DeleteProjectAsync(int? id) {
            Project project = await _database.Projects.Get(id.Value);
            if (project == null)
                throw new ValidationException("project not found", "");
            _database.Projects.Delete(id.Value);
            _database.Save();
        }
        public async Task AppointTeamleadAsync(int? projectId, int? employeeId) {
            var project = await _database.Projects.Get(projectId.Value);
            if (project == null)
                throw new ValidationException("project not found", "");
            
            var employee = await _database.Employees.Get(employeeId.Value);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            project.TeamLead = employee;
            _database.Save();
        }

        public async Task TieEmployeeProjectAsync(int? projectId, int? employeeId) {
            var project = await _database.Projects.Get(projectId.Value);
            if (project == null)
                throw new ValidationException("project is not found", "");

            var employee = await _database.Employees.Get(employeeId.Value);
            if (employee == null)
                throw new ValidationException("employee is not found", "");
            project.Employees?.Add(employee);
            _database.Save();
        }        
        
        public async Task SeparateEmployeeProjectAsync(int? projectId, int? employeeId) {
            var project = await _database.Projects.Get(projectId.Value);
            if (project == null)
                throw new ValidationException("project not found", "");

            var employee = await _database.Employees.Get(employeeId.Value);
            if (employee == null)
                throw new ValidationException("employee not found", "");
            if (project.TeamLead?.Id == employee.Id)
                project.TeamLead = null;
            project.Employees?.Remove(employee);
            _database.Save();
        }

    }
}