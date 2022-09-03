using LKDin.DTOs;
using LKDin.Server.IBusinessLogic;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {

        }

        public UserDTO CreateUser(UserDTO userDTO)
        {
            return null;
        }
    }
}