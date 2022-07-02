﻿using EventPlannerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerAPI.Data;

public class DataContext: DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<UserProject> UsersProjects { get; set; }

    public DbSet<Event> Events { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        // Create, if doesn't exist
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasIndex(user => new {user.Email, user.Username}).IsUnique();
        // Complex pk
        builder.Entity<UserProject>().HasKey(userProject => new { userProject.UserId, userProject.ProjectId });
        // One to many (Users to UsersProjects)
        builder.Entity<UserProject>()
            .HasOne(userProject => userProject.User)
            .WithMany(user => user.UserProjects);
        // One to many (Projects to UsersProjects)
        builder.Entity<UserProject>()
            .HasOne(userProject => userProject.Project)
            .WithMany(project => project.UsersProject);
    }
}