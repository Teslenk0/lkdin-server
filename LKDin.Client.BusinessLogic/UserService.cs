using LKDin.DTOs;
using LKDin.Helpers;
using LKDin.IBusinessLogic;
using LKDin.Networking;

namespace LKDin.Server.BusinessLogic
{
    public class UserClientService : IUserService
    {
        private readonly NetworkDataHelper _networkDataHelper;

        public UserClientService(NetworkDataHelper networkDataHelper)
        {
            this._networkDataHelper = networkDataHelper;
        }

        public void CreateUser(UserDTO userDTO)
        {
            var serializedUser = SerializationManager.Serialize<UserDTO>(userDTO);

            this._networkDataHelper.SendMessage(serializedUser, AvailableOperation.CREATE_USER);

            this._networkDataHelper.ReceiveMessage();
        }

        public UserDTO? GetUser(string userId)
        {
            throw new NotImplementedException();
        }

        public UserDTO ValidateUserCredentials(string userId, string password)
        {
            throw new NotImplementedException();
        }
    }
}