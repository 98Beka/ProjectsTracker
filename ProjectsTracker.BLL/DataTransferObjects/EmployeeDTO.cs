namespace ProjectsTracker.BLL.DataTransferObjects {
    public class EmployeeDTO {
        public int Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? MiddleName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public List<ProjectDTO>? Projects { get; set; } = new();
        public List<ProjectDTO>? ProjectsAsLead { get; set; } = new();
    }
}
