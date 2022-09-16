using LKDin.DTOs;

namespace LKDin.IBusinessLogic
{
    public interface IUserService
    {
        public void CreateUser(UserDTO userDTO);

        public UserDTO GetUser(string userId);

        public UserDTO ValidateUserCredentials(string userId, string password);
    }
}
