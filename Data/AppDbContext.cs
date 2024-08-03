using Microsoft.EntityFrameworkCore;

namespace jo_azure_web_app.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Person> persons { get; set; }
    }
}