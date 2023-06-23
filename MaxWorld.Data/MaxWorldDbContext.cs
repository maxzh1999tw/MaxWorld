using Microsoft.EntityFrameworkCore;
using MaxWorld.Data.Users;

namespace MaxWorld.Data
{
    public class MaxWorldDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserPassword> UserPassword { get; set; }

        public MaxWorldDbContext(DbContextOptions options) : base(options) { }
    }
}
