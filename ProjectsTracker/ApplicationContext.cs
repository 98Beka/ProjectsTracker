﻿using Microsoft.EntityFrameworkCore;
using ProjectsTracker.Models;
public class ApplicationContext : DbContext {
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public ApplicationContext(DbContextOptions options): base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Project>()
            .HasData(new Project { Id = 1, Name = "First" });
        modelBuilder
            .Entity<Employee>()
            .HasMany(e => e.Projects)
            .WithMany(p => p.Employees);
        modelBuilder
            .Entity<Project>()
            .HasOne(p => p.TeamLead)
            .WithMany(e => e.ProjectsAsLead);
    }
}