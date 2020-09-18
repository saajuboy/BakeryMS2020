using System;
using System.Threading.Tasks;
using AutoMapper;
using BakeryMS.API.Business.Interfaces;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Data.Repositories;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Business.Component
{
    public class UserComponent : IUserComponent
    {
        private readonly IAuthRepository _repository;
        private readonly IMapper _mapper;
        public UserComponent(IAuthRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }
        public async Task<User> RegisterUser(UserForRegisterDto userForRegisterDto)
        {
            
            User userToCreate = new User();
            userToCreate = _mapper.Map<User>(userForRegisterDto);

            //dynamic data
            userToCreate.Status = true;

            return await _repository.Register(userToCreate, userForRegisterDto.Password);
        }

        public async Task<User> UpdateUser(UserForRegisterDto userForRegisterDto)
        {
            int a = 0;
            await Task.Run(() => a = 1);
            throw new System.NotImplementedException();
        }
    }
}