using LKDin.DTOs;

namespace LKDin.IBusinessLogic
{
    public interface IUserService
    {
        public Task CreateUser(UserDTO userDTO);

        public Task<UserDTO> GetUser(string userId);

        public Task<UserDTO> ValidateUserCredentials(string userId, string password);
    }
}
