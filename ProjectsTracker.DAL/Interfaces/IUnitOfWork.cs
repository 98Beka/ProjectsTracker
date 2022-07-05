using ProjectsTracker.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsTracker.DAL.Interfaces {
    public interface IUnitOfWork : IDisposable {
        IRepository<Project> Projects { get; }
        IRepository<Employee> Employees { get; }
        void Save();
    }
}
