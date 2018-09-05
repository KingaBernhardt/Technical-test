using AllAboutDough.Repositories;
using AllAboutDough.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllAboutDough
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=AllAboutDough;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<OrderDbContext>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<OrderService>();
        }
    }
}