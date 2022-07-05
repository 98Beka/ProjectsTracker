﻿using Microsoft.EntityFrameworkCore;
using ProjectsTracker.DAL.Interfaces;
using ProjectsTracker.Models;

namespace ProjectsTracker.DAL.Repository {
    public class EFUnitOfWork : IUnitOfWork {
        private ApplicationContext db;
        private ProjectRepository projectRepository;
        private EmployeeRepository employeeRepository;

        public EFUnitOfWork(ApplicationContext db) {
            this.db = db;
        }
        public IRepository<Project> Projects {
            get {
                if (projectRepository == null)
                    projectRepository = new ProjectRepository(db);
                return projectRepository;
            }
        }

        public IRepository<Employee> Employees {
            get {
                if (employeeRepository == null)
                    employeeRepository = new EmployeeRepository(db);
                return employeeRepository;
            }
        }

        public void Save() {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
