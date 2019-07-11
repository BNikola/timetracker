using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerApp.Domain;

namespace TimeTrackerApp.Data
{
    public class TimeTrackerDbContext : DbContext
    {
        public TimeTrackerDbContext(DbContextOptions<TimeTrackerDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Joni", HourRate = 15m },
                new User { Id = 2, Name = "Bravo", HourRate = 20m });

            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Name = "Client 1" },
                new Client { Id = 2, Name = "Client 2" });

            modelBuilder.Entity<Project>().HasData(
                new { Id = 1L, Name = "Project 1", ClientId = 1L },
                new { Id = 2L, Name = "Project 2", ClientId = 1L },
                new { Id = 3L, Name = "Project 3", ClientId = 2L });
        }
    }
}
