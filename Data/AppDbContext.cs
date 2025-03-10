using CloudBased_TaskManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudBased_TaskManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        // Initializes the database context with options for configuration.
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { } // end constructor

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Name      : OnModelCreating(ModelBuilder modelBuilder)
            // Purpose   : Configures the entity relationships.
            // Re-use    : Called automatically during model creation.
            // Input     : ModelBuilder modelBuilder
            //            - The model builder used to configure entity relationships.
            // Output    : Configures the User-Task relationship.
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);
        } // end OnModelCreating
    } // end AppDbContext
}
