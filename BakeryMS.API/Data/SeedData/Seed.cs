using System;
using System.Collections.Generic;
using System.Linq;
using BakeryMS.API.Models;
using BakeryMS.API.Models.Inventory;
using BakeryMS.API.Models.Production;
using BakeryMS.API.Models.Profile;
using Newtonsoft.Json;

namespace BakeryMS.API.Data.SeedData
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            if (!_context.Users.Any())
            {
                SeedUsersAndRoles();
            }
            if (!_context.Items.Any())
            {
                SeedItemsCategoriesANDUnits();
            }
            if (!_context.Suppliers.Any())
            {
                SeedSupplier();
            }
            if (!_context.PurchaseOrderHeaders.Any())
            {
                SeedPurchaseOrder();
            }
            if (!_context.BusinessPlaces.Any())
            {
                SeedBusinessPlace();
            }
            if (!_context.ProductionSessions.Any())
            {
                SeedProductionOrderAndSession();
            }
        }

        private void SeedProductionOrderAndSession()
        {
            // SupplierSeedData.Json
            IList<ProductionSession> defaultProductionSessions = new List<ProductionSession>();

            defaultProductionSessions.Add(new ProductionSession() { Session = "Special 2", StartTime = new TimeSpan(20, 0, 0), EndTime = new TimeSpan(23, 0, 0) });
            defaultProductionSessions.Add(new ProductionSession() { Session = "Special 1", StartTime = new TimeSpan(6, 0, 0), EndTime = new TimeSpan(12, 0, 0) });
            defaultProductionSessions.Add(new ProductionSession() { Session = "Evening", StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(17, 0, 0) });
            defaultProductionSessions.Add(new ProductionSession() { Session = "Morning", StartTime = new TimeSpan(1, 0, 0), EndTime = new TimeSpan(5, 0, 0) });
            _context.ProductionSessions.AddRange(defaultProductionSessions);
            _context.SaveChanges();



            var productionItems = _context.Items.Where(a => a.Type == 0).ToList();

            IList<ProductionOrderDetail> defaultProductionOrderDetails1 = new List<ProductionOrderDetail>();

            defaultProductionOrderDetails1.Add(new ProductionOrderDetail() { Item = productionItems.ElementAt(0), Quantity = 4, Description = "Soft1" });
            defaultProductionOrderDetails1.Add(new ProductionOrderDetail() { Item = productionItems.ElementAt(1), Quantity = 4, Description = "Soft2" });

            IList<ProductionOrderDetail> defaultProductionOrderDetails2 = new List<ProductionOrderDetail>();

            defaultProductionOrderDetails1.Add(new ProductionOrderDetail() { Item = productionItems.ElementAt(2), Quantity = 4, Description = "Soft3" });
            defaultProductionOrderDetails1.Add(new ProductionOrderDetail() { Item = productionItems.ElementAt(3), Quantity = 4, Description = "Soft4" });



            var user = _context.Users.FirstOrDefault(a => a.Username == "saajidh");
            var businessPlaces = _context.BusinessPlaces.ToList();
            var sessions = _context.ProductionSessions.ToList();

            IList<ProductionOrderHeader> defaultProductionOrders = new List<ProductionOrderHeader>();

            defaultProductionOrders.Add(new ProductionOrderHeader()
            {
                ProductionOrderNo = 2,
                Session = sessions.ElementAt(1),
                User = user,
                EnteredDate = DateTime.Today,
                RequiredDate = DateTime.Today.AddDays(4),
                BusinessPlace = businessPlaces.ElementAt(1),
                ProductionOrderDetails = defaultProductionOrderDetails2
            });
            defaultProductionOrders.Add(new ProductionOrderHeader()
            {
                ProductionOrderNo = 1,
                Session = sessions.ElementAt(0),
                User = user,
                EnteredDate = DateTime.Today,
                RequiredDate = DateTime.Today.AddDays(3),
                BusinessPlace = businessPlaces.ElementAt(0),
                ProductionOrderDetails = defaultProductionOrderDetails1
            });



            _context.ProductionOrderHeaders.AddRange(defaultProductionOrders);
            _context.SaveChanges();
        }
        private void SeedBusinessPlace()
        {
            IList<BusinessPlace> defaultBusinessPlaces = new List<BusinessPlace>();

            defaultBusinessPlaces.Add(new BusinessPlace() { Name = "Nanu Oya Bakery", Address = "No 17 Main Street NuwaraEliya " });
            defaultBusinessPlaces.Add(new BusinessPlace() { Name = "Main Outlet", Address = "No 17 Main Street NuwaraEliya " });
            defaultBusinessPlaces.Add(new BusinessPlace() { Name = "Bus Stand Outlet", Address = "No 17 Main Street NuwaraEliya " });
            defaultBusinessPlaces.Add(new BusinessPlace() { Name = "Mosque Outlet", Address = "No 17 Main Street NuwaraEliya " });

            _context.BusinessPlaces.AddRange(defaultBusinessPlaces);
            _context.SaveChanges();
        }
        private void SeedSupplier()
        {
            // SupplierSeedData.Json
            var SupData = System.IO.File.ReadAllText("Data/SeedData/Master/SupplierSeedData.Json");
            var Suppliers = JsonConvert.DeserializeObject<List<Supplier>>(SupData);

            foreach (var sup in Suppliers)
            {
                _context.AddRange(sup);
                _context.SaveChanges();
            }
        }
        private void SeedPurchaseOrder()
        {
            var POData = System.IO.File.ReadAllText("Data/SeedData/Inventory/PurchaseOrderSeedData.Json");
            var POHs = JsonConvert.DeserializeObject<List<PurchaseOrderHeader>>(POData);

            foreach (var POH in POHs)
            {
                if (POH.PurchaseOrderDetail.Any())
                {
                    foreach (var detail in POH.PurchaseOrderDetail)
                    {
                        detail.Item = _context.Items
                        .Where(a => a.Name == detail.Item.Name)
                        .FirstOrDefault();
                    }
                }

                _context.AddRange(POH);
                _context.SaveChanges();
            }
        }
        private void SeedItemsCategoriesANDUnits()
        {
            var itemData = System.IO.File.ReadAllText("Data/SeedData/Master/ItemSeedData.Json");
            var items = JsonConvert.DeserializeObject<List<Item>>(itemData);

            foreach (var item in items)
            {
                if (_context.ItemCategories.Any(a => a.Description == item.ItemCategory.Description))
                {
                    item.ItemCategory = _context.ItemCategories
                    .Where(a => a.Description == item.ItemCategory.Description)
                    .FirstOrDefault();
                }
                if (_context.Units.Any(a => a.Description == item.Unit.Description))
                {
                    item.Unit = _context.Units
                    .Where(a => a.Description == item.Unit.Description)
                    .FirstOrDefault();
                }
                _context.AddRange(item);
                _context.SaveChanges();
            }
            // _context.AddRange(items);

        }
        private void SeedUsersAndRoles()
        {
            var userData = System.IO.File.ReadAllText("Data/SeedData/Profile/UserSeedData.Json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            var roleData = System.IO.File.ReadAllText("Data/SeedData/Profile/RolesSeedData");
            var roles = JsonConvert.DeserializeObject<List<Roles>>(roleData);
            var count = 1;

            foreach (var role in roles)
            {
                _context.Roles.Add(role);
            }
            _context.SaveChanges();

            foreach (var user in users)
            {
                var UsrRlsMappng = new UserRolesMapping();

                byte[] passwordHash, passwordSalt;
                CreatePasswordhash("password", out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                if (count == 1)
                {
                    UsrRlsMappng.User = user;
                    UsrRlsMappng.Roles = _context.Roles.Where(a => a.RoleName == "Admin").FirstOrDefault();
                    _context.AddRange(user, UsrRlsMappng);
                    _context.SaveChanges();
                }
                if (count == 2)
                {
                    UsrRlsMappng.User = user;
                    UsrRlsMappng.Roles = _context.Roles.Where(a => a.RoleName == "BakeryManager").FirstOrDefault();
                    _context.AddRange(user, UsrRlsMappng);
                    _context.SaveChanges();
                }
                if (count == 3)
                {
                    UsrRlsMappng.User = user;
                    UsrRlsMappng.Roles = _context.Roles.Where(a => a.RoleName == "OutletManager").FirstOrDefault();
                    _context.AddRange(user, UsrRlsMappng);
                    _context.SaveChanges();

                }

                UsrRlsMappng.User = user;
                UsrRlsMappng.Roles = _context.Roles.Where(a => a.RoleName == "User").FirstOrDefault();
                _context.AddRange(user, UsrRlsMappng);
                _context.SaveChanges();

            }

            // _context.SaveChanges();
        }

        private void CreatePasswordhash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


    }
}