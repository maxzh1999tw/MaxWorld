using MaxWorld.Data.Exercises;
using MaxWorld.Data.Users;
using Microsoft.EntityFrameworkCore;

namespace MaxWorld.Data
{
    public class MaxWorldDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserPassword> UserPassword { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Exercise> Exercise { get; set; }

        public MaxWorldDbContext(DbContextOptions options) : base(options) { }
    }
}
