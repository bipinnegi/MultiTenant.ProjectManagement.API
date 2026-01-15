using Microsoft.EntityFrameworkCore;
using MultiTenant.ProjectManagement.API.Models;

namespace MultiTenant.ProjectManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Invitation> Invitations {  get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
    }
}

