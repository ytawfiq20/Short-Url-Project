

using Microsoft.EntityFrameworkCore;
using UrlShorten.Data.Entities;
using UrlShorten.Data.EntityConfiguration;

namespace UrlShorten.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UrlMapperConfiguration());
        }

        public DbSet<UrlMapper> UrlMappers { get; set; }

    }
}
