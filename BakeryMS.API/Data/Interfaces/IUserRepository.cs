using System.Collections.Generic;
using System.Threading.Tasks;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
    }
}