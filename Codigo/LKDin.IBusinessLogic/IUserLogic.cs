using LKDin.DTOs;

namespace LKDin.IBusinessLogic
{
    public interface IUserLogic
    {
        public Task CreateUser(UserDTO userDTO);

        public Task UpdateUser(UserDTO userDTO);

        public Task DeleteUser(UserDTO userDTO);

        public Task<UserDTO> GetUser(string userId);

        public Task<UserDTO> ValidateUserCredentials(string userId, string password);
    }
}
