using System.Collections.Generic;
using System.Linq;
using BakeryMS.API.Models;
using BakeryMS.API.Models.Inventory;
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
            if (!_context.PurchaseOrderHeaders.Any())
            {
                SeedPurchaseOrder();
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