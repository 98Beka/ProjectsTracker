
using AutoMapper;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.PL.ViewModels;

namespace ProjectsTracker.BLL.Profiles {
    public class EmployeeMapperConfig : Profile {
        public EmployeeMapperConfig() {
            CreateMap<EmployeeDTO, EmployeeView>();
            CreateMap<EmployeeView, EmployeeDTO>();
        }
    }
}
