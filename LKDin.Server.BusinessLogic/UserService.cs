using LKDin.DTOs;
using LKDin.Exceptions;
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

            var exists = this._userRepository.Exists(userDTO.Id);

            if (exists)
            {
                throw new UserAlreadyExistsException(userDTO.Id);
            }

            var user = UserDTO.DTOToEntity(userDTO);

            user.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            this._userRepository.Create(user);
        }

        public UserDTO? GetUser(string userId)
        {

            var user = this._userRepository.Get(userId);

            if(user != null)
            {
                return UserDTO.EntityToDTO(user);
            }

            return null;
        }

        public UserDTO ValidateUserCredentials(string userId, string password)
        {
            var user = this._userRepository.Get(userId);

            if (user == null)
            {
                throw new UserDoesNotExistException(userId);
            }

            bool verified = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!verified)
            {
                throw new UnauthorizedException();
            }

            return UserDTO.EntityToDTO(user);
        }
    }
}