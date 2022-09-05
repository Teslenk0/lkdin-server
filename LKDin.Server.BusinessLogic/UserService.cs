﻿using LKDin.DTOs;
using LKDin.IBusinessLogic;
using LKDin.Server.DataAccess.Repositories;
using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            this._userRepository = new UserRepository();
        }

        public void CreateUser(UserDTO userDTO)
        {
            var user = new User()
            {
                Name = userDTO.Name,
                Password = userDTO.Password,
                Id = userDTO.Id
            };

            this._userRepository.Create(user);
        }
    }
}