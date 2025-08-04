using Jwt_ApiVersioning.Models;
using Microsoft.EntityFrameworkCore;

namespace Jwt_ApiVersioning.Context
{
    public class ApiContext:DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options):base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
