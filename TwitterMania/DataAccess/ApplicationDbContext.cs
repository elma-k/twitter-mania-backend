using Microsoft.EntityFrameworkCore;
using TwitterMania.Model;

namespace TwitterMania.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        public DbSet<TweetModel> Tweet { get; set; }
        public DbSet<UserModel> User { get; set; }

    }
}
