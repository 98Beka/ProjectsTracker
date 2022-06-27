namespace ProjectsTracker.Models {
    public class Employee {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Project>? Projects { get; set; } = new();
        public List<Project>? ProjectsAsLead { get; set; } = new();
    }
}
