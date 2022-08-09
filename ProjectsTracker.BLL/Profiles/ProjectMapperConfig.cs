using AutoMapper;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.DAL.Models;

namespace ProjectsTracker.BLL.Profiles {
    public class ProjectMapperConfig : Profile {
        public ProjectMapperConfig() {
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectDTO, Project>()
                .ForMember(m => m.Employees, o => o.Ignore())
                .ForMember(m => m.TeamLead, o => o.Ignore());
        }
    }
}
