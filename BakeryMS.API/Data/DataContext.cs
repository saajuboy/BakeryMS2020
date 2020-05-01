using BakeryMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Unit> Units { get; set; }
    }
}