using System.Collections.Generic;
using System.Threading.Tasks;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Data.Interfaces
{
    public interface IUserRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
        Task<User> UpdateUser(User user, string password);
        Task<bool> UserExists(string username, int id);
        Task<bool> DeleteUser(int id);
    }
}