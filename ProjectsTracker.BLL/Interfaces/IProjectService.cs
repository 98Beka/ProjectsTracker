using ProjectsTracker.BLL.DataTransferObjects;

namespace ProjectsTracker.BLL.Interfaces {
    public interface IProjectService {
        Task<ProjectDTO> GetProjectAsync(int id);
        IEnumerable<ProjectDTO> GetProjects(IProjectFilter filter);
        Task DeleteProjectAsync(int id);
        Task<ProjectDTO> AddOrEditProjectAsync(ProjectDTO project);
        Task AppointTeamleadAsync (int projectId, int employeeId);
        Task TieEmployeeProjectAsync(int projectId, int employeeId);
        Task SeparateEmployeeProjectAsync(int projectId, int employeeId);
    }
}
