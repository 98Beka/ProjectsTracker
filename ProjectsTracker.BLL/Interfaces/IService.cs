using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsTracker.BLL.Interfaces {
    public interface IService : IEmployeeService, IProjectService {
        Task TieEmployeeProjectAsync(int? projectId, int? employeeId);
        Task SeparateEmployeeProjectAsync(int? projectId, int? employeeId);
    }
}
