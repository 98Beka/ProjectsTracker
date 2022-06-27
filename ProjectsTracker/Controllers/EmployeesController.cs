using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsTracker.Models;

namespace ProjectsTracker.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase {
        private ApplicationContext db;

        public EmployeesController(ApplicationContext db) =>
            this.db = db;

        [HttpGet]
        public async Task<List<Employee>> Get() => await db.Employees.ToListAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var result = await db.Employees.Include(e => e.Projects).FirstOrDefaultAsync(p => p.Id == id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var entity = await db.Employees.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
                return NotFound();
            db.Employees.Remove(entity);
            await db.SaveChangesAsync();
            return Ok(new { Message = "Deleted successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> Post(Employee employee) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await db.Employees.AddAsync(employee);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }
        [HttpPost("PostBody")]
        public async Task<IActionResult> PostBody([FromBody] Employee project) =>
            await Post(project);

        [HttpPut]
        public async Task<IActionResult> Put(Employee employee) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var storedProject = await db.Employees.FirstOrDefaultAsync(p => p.Id == employee.Id);
            if (storedProject == null)
                return NotFound();
            storedProject.Name = employee.Name;
            await db.SaveChangesAsync();
            return Ok(storedProject);
        }

        [HttpGet("addProject")]
        public async Task<IActionResult> AddProject(int employeeId, int projectId) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var storedEmployee = await db.Employees.Include(p => p.Projects).FirstOrDefaultAsync(e => e.Id == employeeId);
            var project = await db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null || storedEmployee == null)
                return NotFound();
            storedEmployee.Projects?.Add(project);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("removeProject")]
        public async Task<IActionResult> RemoveProject(int employeeId, int projectId) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var storedEmployee = await db.Employees.Include(p => p.Projects).FirstOrDefaultAsync(e => e.Id == employeeId);
            if (storedEmployee == null)
                return NotFound();
            foreach (var p in storedEmployee.Projects.ToList())
                if (p.Id == projectId)
                    storedEmployee.Projects?.Remove(p);
            await db.SaveChangesAsync();
            return Ok(storedEmployee);
        }

    }
}