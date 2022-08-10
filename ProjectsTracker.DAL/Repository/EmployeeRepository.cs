using Microsoft.EntityFrameworkCore;
using ProjectsTracker.DAL.Interfaces;
using ProjectsTracker.DAL.Models;


namespace ProjectsTracker.DAL.Repository {
    internal class EmployeeRepository : IRepository<Employee> {
        private ApplicationContext db;
        public EmployeeRepository(ApplicationContext context) {
            this.db = context;
        }

        public IEnumerable<Employee> GetAll() {
            return db.Employees.Include(e => e.Projects).Include(p => p.ProjectsAsLead);
        }

        public async Task<Employee> Get(int id) {
            return await db.Employees.Include(e => e.Projects).Include(p => p.ProjectsAsLead).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> Create(Employee employee) {
            return (await db.Employees.AddAsync(employee)).Entity;
        }

        public void Update(Employee employee) {
            db.Entry(employee).State = EntityState.Modified;
        }

        public IEnumerable<Employee> Find(Func<Employee, Boolean> predicate) {
            return db.Employees.Include(e => e.Projects).Include(p => p.ProjectsAsLead).Where(predicate).ToList();
        }

        public void Delete(int id) {
            Employee employee = db.Employees.Find(id);
            if (employee != null)
                db.Employees.Remove(employee);
        }
    }
}
