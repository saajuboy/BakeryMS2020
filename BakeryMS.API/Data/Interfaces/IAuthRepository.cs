using System.Threading.Tasks;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Data.Interfaces
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}