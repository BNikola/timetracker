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
    }
}
