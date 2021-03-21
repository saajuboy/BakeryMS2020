using System.Linq;
using BakeryMS.API.Models;
using BakeryMS.API.Models.HumanResource;
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
        public DbSet<Machinery> Machineries { get; set; }
        public DbSet<ProductionItem> ProductionItems { get; set; }
        public DbSet<CompanyItem> CompanyItems { get; set; }
        public DbSet<RawItems> RawItems { get; set; }

        //User,Roles and User Roles Mapping
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRolesMapping> UserRolesMappings { get; set; }

        //PurchaseOrder Header and detail,GRN header and Detail
        public DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<GRNHeader> GRNHeaders { get; set; }
        public DbSet<GRNDetail> GRNDetails { get; set; }

        //Production Order Header,detail, Session and Ingredients
        public DbSet<ProductionOrderHeader> ProductionOrderHeaders { get; set; }
        public DbSet<ProductionOrderDetail> ProductionOrderDetails { get; set; }
        public DbSet<ProductionSession> ProductionSessions { get; set; }
        public DbSet<IngredientHeader> IngredientHeaders { get; set; }
        public DbSet<IngredientDetail> IngredientDetails { get; set; }

        //Production Plan
        public DbSet<ProductionPlanHeader> ProductionPlanHeaders { get; set; }
        public DbSet<ProductionPlanDetail> ProductionPlanDetails { get; set; }
        public DbSet<ProductionPlanRecipe> ProductionPlanRecipes { get; set; }
        public DbSet<ProductionPlanWorker> ProductionPlanWorkers { get; set; }
        public DbSet<ProductionPlanMachine> ProductionPlanMachines { get; set; }

        //Employee and Routine
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Routine> Routines { get; set; }

    }
}