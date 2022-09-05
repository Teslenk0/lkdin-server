using LKDin.DTOs;

namespace LKDin.IBusinessLogic
{
    public interface IUserService
    {
        public void CreateUser(UserDTO userDTO);

        public UserDTO ValidateUserCredentials(string userId, string password);
    }
}
