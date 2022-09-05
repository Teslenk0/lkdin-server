using LKDin.DTOs;

namespace LKDin.Server.IBusinessLogic
{
    public interface IUserService
    {
        public void CreateUser(UserDTO userDTO);
    }
}
