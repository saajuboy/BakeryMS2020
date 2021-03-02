using System.Linq;
using BakeryMS.API.Models;
using BakeryMS.API.Models.Inventory;
using BakeryMS.API.Models.Production;
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
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<BusinessPlace> BusinessPlaces { get; set; }

        //User,Roles and User Roles Mapping
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRolesMapping> UserRolesMappings { get; set; }

        //PurchaseOrder Header and detail
        public DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        //Production Order Header,detail and Session
        public DbSet<ProductionOrderHeader> ProductionOrderHeaders { get; set; }
        public DbSet<ProductionOrderDetail> ProductionOrderDetails { get; set; }
        public DbSet<ProductionSession> ProductionSessions { get; set; }

        //Ingredients
        public DbSet<IngredientHeader> IngredientHeaders { get; set; }
        public DbSet<IngredientDetail> IngredientDetails { get; set; }
    }
}