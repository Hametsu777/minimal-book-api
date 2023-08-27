global using Microsoft.EntityFrameworkCore;

namespace MinimalBookApi
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=minimalbookdb;User ID=postgres;Password=admin;");
        }

        // Remember to check out Set Method.
        public DbSet<Book> Books => Set<Book>();
    }
}
