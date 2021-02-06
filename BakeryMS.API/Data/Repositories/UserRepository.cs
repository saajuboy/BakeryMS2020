using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.Profile;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<User> GetUser(int id)
        {
            return await _context.Users.Where(a => a.IsDeleted == false).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Where(a => a.IsDeleted == false).ToListAsync();
        }


        public async Task<bool> UserExists(string username, int id)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username && x.Id != id))
                return true;

            return false;
        }

        public async Task<User> UpdateUser(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordhash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            if (await _context.SaveChangesAsync() > 0)
            {
                return user;
            }
            else
            {
                return null;
            }
            // await _context.SaveChangesAsync();

        }

        private void CreatePasswordhash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> DeleteUser(int id)
        {

            var user = await _context.Users.Where(a => a.IsDeleted == false).FirstOrDefaultAsync(a => a.Id == id);
            if (user != null)
            {
                user.IsDeleted = true;
                if (await _context.SaveChangesAsync() > 0)
                    return true;
                return false;
            }
            return false;

        }
    }
}