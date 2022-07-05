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
            return db.Projects.Include(p => p.Employees);
        }

        public async Task<Project> Get(int id) {
            return await db.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Create(Project Project) {
            await db.Projects.AddAsync(Project);
        }

        public void Update(Project Project) {
            db.Entry(Project).State = EntityState.Modified;
        }
        public IEnumerable<Project> Find(Func<Project, Boolean> predicate) {
            return db.Projects.Include(p => p.Employees).Where(predicate).ToList();
        }
        public void Delete(int id) {
            Project Project = db.Projects.Find(id);
            if (Project != null)
                db.Projects.Remove(Project);
        }
    }
}
