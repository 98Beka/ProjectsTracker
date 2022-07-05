using ProjectsTracker.BLL.DataTransferObjects;

namespace ProjectsTracker.BLL.Interfaces {
    public interface IProjectFilter {
        public List<ProjectDTO> Filtrate(List<ProjectDTO> input);
    }
}
