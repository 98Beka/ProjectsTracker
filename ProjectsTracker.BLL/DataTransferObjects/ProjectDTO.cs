using ProjectsTracker.DAL.Models;

namespace ProjectsTracker.BLL.DataTransferObjects {
    public class ProjectDTO {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? CustomerName { get; set; } = null!;
        public string? PerformerName { get; set; } = null!;
        public List<EmployeeDTO>? Employees { get; set; } = new();
        public EmployeeDTO? TeamLead { get; set; }
        public DateTime Start { get; set; } = new();
        public DateTime Finish { get; set; } = new();
        public int? Priority { get; set; } = new();
    }
}
