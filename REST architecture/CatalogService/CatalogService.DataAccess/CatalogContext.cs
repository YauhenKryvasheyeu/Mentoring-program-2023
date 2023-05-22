using CatalogService.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.DataAccess
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }
    }
}