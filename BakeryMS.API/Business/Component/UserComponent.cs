using System;
using System.Threading.Tasks;
using BakeryMS.API.Business.Interfaces;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Data.Repositories;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Business.Component
{
    public class UserComponent : IUserComponent
    {
        private readonly User _userToCreate;
        private readonly IAuthRepository _repository;
        public UserComponent(IAuthRepository repository)
        {
            _repository = repository;

            User userToCreate = new User();
            _userToCreate = userToCreate;

        }
        public async Task<User> RegisterUser(UserForRegisterDto userForRegisterDto)
        {

            //static data
            _userToCreate.Username = userForRegisterDto.Username.ToLower();
            _userToCreate.FirstName = userForRegisterDto.FirstName;
            _userToCreate.LastName = userForRegisterDto.LastName;
            _userToCreate.Gender = userForRegisterDto.Gender;
            _userToCreate.DateOfBirth = userForRegisterDto.DateOfBirth;
            _userToCreate.ContactNumber = userForRegisterDto.ContactNumber;
            _userToCreate.PhotoUrl = userForRegisterDto.PhotoUrl;
            _userToCreate.PhotoPublicId = userForRegisterDto.PhotoPublicId;

            //dynamic data
            _userToCreate.Status = true;
            _userToCreate.Created = DateTime.Now;

            return await _repository.Register(_userToCreate, userForRegisterDto.Password);
        }

        public async Task<User> UpdateUser(UserForRegisterDto userForRegisterDto)
        {
            int a = 0;
            await Task.Run(() => a = 1);
            throw new System.NotImplementedException();
        }
    }
}