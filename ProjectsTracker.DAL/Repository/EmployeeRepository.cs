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
            return db.Employees.Include(e => e.Projects);
        }

        public async Task<Employee> Get(int id) {
            return await db.Employees.Include(e => e.Projects).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Create(Employee employee) {
            await db.Employees.AddAsync(employee);
        }

        public void Update(Employee employee) {
            db.Entry(employee).State = EntityState.Modified;
        }

        public IEnumerable<Employee> Find(Func<Employee, Boolean> predicate) {
            return db.Employees.Where(predicate).ToList();
        }

        public void Delete(int id) {
            Employee employee = db.Employees.Find(id);
            if (employee != null)
                db.Employees.Remove(employee);
        }
    }
}
