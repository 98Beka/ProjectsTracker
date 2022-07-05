using ProjectsTracker.BLL.DataTransferObjects;

namespace ProjectsTracker.BLL.Interfaces {
    public interface IEmployeeService {
        Task<EmployeeDTO> GetEmployeeAsync(int? id);
        IEnumerable<EmployeeDTO> GetEmployees();
        Task DeleteEmployeeAsync(int? id);
        Task AddOrEditEmployeeAsync(EmployeeDTO project);
    }
}
