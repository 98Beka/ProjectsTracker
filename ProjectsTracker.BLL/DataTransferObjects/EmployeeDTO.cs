using ProjectsTracker.DAL.Models;

namespace ProjectsTracker.BLL.DataTransferObjects {
    public class EmployeeDTO {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? MidleName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public List<Project>? Projects { get; set; } = new();
        public List<Project>? ProjectsAsLead { get; set; } = new();
    }
}
