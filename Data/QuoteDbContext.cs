using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers.Models;

namespace WebApplication1.Data
{
    public class QuoteDbContext : DbContext
    {
        public QuoteDbContext(DbContextOptions<QuoteDbContext> options) : base(options) { }

        public DbSet<Quote> Quotes { get; set; }

    }
}
