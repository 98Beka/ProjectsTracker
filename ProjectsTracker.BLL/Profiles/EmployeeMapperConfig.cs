
using AutoMapper;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.DAL.Models;

namespace ProjectsTracker.BLL.Profiles {
    public class EmployeeMapperConfig : Profile {
        public EmployeeMapperConfig() {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeDTO, Employee>();
        }
    }
}
