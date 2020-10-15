using System.Linq;
using BakeryMS.API.Models;
using BakeryMS.API.Models.Inventory;
using BakeryMS.API.Models.Profile;
using Microsoft.EntityFrameworkCore;


namespace BakeryMS.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Unit> Units { get; set; }

        //User,Roles and User Roles Mapping
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRolesMapping> UserRolesMappings { get; set; }
        public DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

    }
}