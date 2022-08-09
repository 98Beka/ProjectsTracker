using AutoMapper;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.PL.ViewModels;

namespace ProjectsTracker.BLL.Profiles {
    public class ProjectMapperConfig : Profile {
        public ProjectMapperConfig() {
            CreateMap<ProjectView, ProjectDTO>()
                .ForMember(m => m.Employees, o => o.Ignore())
                .ForMember(m => m.TeamLead, o => o.Ignore());
            CreateMap<ProjectDTO, ProjectView>();
        }
    }
}
