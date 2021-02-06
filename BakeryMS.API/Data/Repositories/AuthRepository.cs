using System;
using System.Linq;
using System.Threading.Tasks;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.Profile;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly UserRolesMapping _userRolesMapping;
        public AuthRepository(DataContext context)
        {
            UserRolesMapping userRolesMapping = new UserRolesMapping();
            
            _userRolesMapping = userRolesMapping;
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.Where(a=>a.IsDeleted == false).FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!verifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            user.LastActive = DateTime.Now;
            _context.SaveChanges();

            return user;
        }

        private bool verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordhash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var role = await _context.Roles.FirstOrDefaultAsync(a => a.RoleName == "User");
            _userRolesMapping.Roles = role;
            _userRolesMapping.User = user;

            await _context.UserRolesMappings.AddAsync(_userRolesMapping);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordhash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }
    }
}