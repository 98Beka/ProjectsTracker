using ProjectsTracker.DAL.Models;

namespace ProjectsTracker.PL.ViewModels {
    public class ProjectView {
        public int Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? CustomerName { get; set; } = null!;
        public string? PerformerName { get; set; } = null!;
        public DateTime Start { get; set; } = new();
        public DateTime Finish { get; set; } = new();
        public int? Priority { get; set; } = new();
        public List<EmployeeView>? Employees { get; set; } = new();
        public EmployeeView? TeamLead { get; set; }
    }
}
