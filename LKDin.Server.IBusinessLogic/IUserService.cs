using LKDin.DTOs;

namespace LKDin.Server.IBusinessLogic
{
    public interface IUserService
    {
        public UserDTO CreateUser(UserDTO userDTO);
    }
}
