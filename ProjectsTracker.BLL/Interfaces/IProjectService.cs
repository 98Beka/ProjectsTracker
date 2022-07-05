using ProjectsTracker.BLL.DataTransferObjects;

namespace ProjectsTracker.BLL.Interfaces {
    public interface IProjectService {
        Task<ProjectDTO> GetProjectAsync(int? id);
        IEnumerable<ProjectDTO> GetProjects(IProjectFilter filter);
        Task DeleteProjectAsync(int? id);
        Task AddOrEditProjectAsync(ProjectDTO project);
        Task AppointTeamleadAsync (int? projectId, int? employeeId);
    }
}
