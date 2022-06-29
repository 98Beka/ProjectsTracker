using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsTracker.Models;

namespace ProjectsTracker.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class ProjectsController : Controller {
        private ApplicationContext db;

        public ProjectsController(ApplicationContext db) =>
            this.db = db;

        [HttpGet]
        public async Task<List<Project>> Get() =>
            await db.Projects.ToListAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var result = await db.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var entity = await db.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
                return NotFound();
            db.Projects.Remove(entity);
            await db.SaveChangesAsync();
            return Ok(new { Message = "Deleted successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> Post(Project project) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await db.Projects.AddAsync(project);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = project.Id }, project);
        }

        [HttpPost("PostBody")]
        public async Task<IActionResult> PostBody([FromBody] Project project) =>
            await Post(project);

        [HttpPut]
        public async Task<IActionResult> Put(Project project) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var storedProject = await db.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);
            if (storedProject == null)
                return NotFound();
            storedProject.Name = project.Name;
            await db.SaveChangesAsync();
            return Ok(storedProject);
        }

        [HttpGet("addEmployee")]
        public async Task<IActionResult> AddEmployee(int projectId, int employeeId) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var storedProject = await db.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == projectId);
            var emplyee = await db.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (emplyee == null || storedProject == null)
                return NotFound();
            storedProject.Employees?.Add(emplyee);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("appointTeamlead")]
        public async Task<IActionResult> AppointTeamlead(int projectId, int employeeId) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var storedProject = await db.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == projectId);
            var emplyee = await db.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (emplyee == null || storedProject == null)
                return NotFound();
            storedProject.TeamLead = emplyee;
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("removeEmployee")]
        public async Task<IActionResult> RemoveEmployee(int projectId, int employeeId) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var storedProject = await db.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == projectId);
            if (storedProject == null)
                return NotFound();
            foreach (var e in storedProject.Employees.ToList())
                if (e.Id == employeeId)
                    storedProject.Employees?.Remove(e);
            await db.SaveChangesAsync();
            return Ok(storedProject);
        }
    }
}
