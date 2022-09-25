using LKDin.DTOs;
using LKDin.Helpers.Serialization;
using LKDin.IBusinessLogic;
using LKDin.Networking;

namespace LKDin.Client.BusinessLogic
{
    public class UserClientService : IUserService
    {
        private readonly NetworkDataHelper _networkDataHelper;

        public UserClientService(NetworkDataHelper networkDataHelper)
        {
            _networkDataHelper = networkDataHelper;
        }

        public void CreateUser(UserDTO userDTO)
        {
            var serializedUser = SerializationManager.Serialize<UserDTO>(userDTO);

            _networkDataHelper.SendMessage(serializedUser, AvailableOperation.CREATE_USER);

            _networkDataHelper.ReceiveMessage();
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