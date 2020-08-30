using System.Threading.Tasks;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Business.Interfaces
{
    public interface IUserComponent
    {
         Task<User> RegisterUser(UserForRegisterDto userForRegisterDto);
         Task<User> UpdateUser(UserForRegisterDto userForRegisterDto);
    }
}