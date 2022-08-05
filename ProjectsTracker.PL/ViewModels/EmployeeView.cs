namespace ProjectsTracker.PL.ViewModels {
    public class EmployeeView {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? MidleName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public List<ProjectView>? Projects { get; set; } = new();
        public List<ProjectView>? ProjectsAsLead { get; set; } = new();
    }
}
