using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AllAboutDough.Repositories
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Orders> Orders { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }
    }

    public class OrderContextDbFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
         OrderDbContext IDesignTimeDbContextFactory<OrderDbContext>.CreateDbContext(string[] args)
         {
            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectsV13;Initial Catalog=AllAboutDough;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return new OrderDbContext(optionsBuilder.Options);
         }
    }
}
