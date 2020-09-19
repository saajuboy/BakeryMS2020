using System;
using System.Collections.Generic;
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
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserComponent(IAuthRepository repository, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authRepository = repository;

        }
        public async Task<User> RegisterUser(UserForRegisterDto userForRegisterDto)
        {

            User userToCreate = new User();
            userToCreate = _mapper.Map<User>(userForRegisterDto);

            //dynamic data
            userToCreate.Status = true;

            return await _authRepository.Register(userToCreate, userForRegisterDto.Password);
        }

        public async Task<User> UpdateUser(UserForRegisterDto userForRegisterDto)
        {
            int a = 0;
            await Task.Run(() => a = 1);
            throw new System.NotImplementedException();
        }

        public async Task<UserForDetailDto> GetUser(int id)
        {

            var userFromRepository = await _userRepository.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailDto>(userFromRepository);

            return userToReturn;
        }

        public async Task<IEnumerable<UserForDetailDto>> GetUsers()
        {
            var usersFromRepository = await _userRepository.GetUsers();

            var userToReturn = _mapper.Map<IEnumerable<UserForDetailDto>>(usersFromRepository);

            return userToReturn;
        }
    }
}