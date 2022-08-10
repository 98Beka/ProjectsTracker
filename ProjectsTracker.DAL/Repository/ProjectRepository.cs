using Microsoft.EntityFrameworkCore;
using ProjectsTracker.DAL.Interfaces;
using ProjectsTracker.DAL.Models;

namespace ProjectsTracker.DAL.Repository {
    public class ProjectRepository : IRepository<Project> {
        private ApplicationContext db;

        public ProjectRepository(ApplicationContext context) {
            this.db = context;
        }

        public IEnumerable<Project> GetAll() {
            return db.Projects.Include(p => p.Employees).Include(p => p.TeamLead);
        }

        public async Task<Project> Get(int id) {
            return await db.Projects.Include(p => p.Employees).Include(p => p.TeamLead).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> Create(Project Project) {
            return (await db.Projects.AddAsync(Project)).Entity;
        }

        public void Update(Project Project) {
            db.Entry(Project).State = EntityState.Modified;
        }
        public IEnumerable<Project> Find(Func<Project, Boolean> predicate) {
            return db.Projects.Include(p => p.Employees).Include(p => p.TeamLead).Where(predicate).ToList();
        }
        public void Delete(int id) {
            Project Project = db.Projects.Find(id);
            if (Project != null)
                db.Projects.Remove(Project);
        }
    }
}
