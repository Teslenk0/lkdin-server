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

        public async Task CreateUser(UserDTO userDTO)
        {
            var serializedUser = SerializationManager.Serialize<UserDTO>(userDTO);

            await _networkDataHelper.SendMessage(serializedUser, AvailableOperation.CREATE_USER);

            await _networkDataHelper.ReceiveMessage();
        }

        public async Task <UserDTO?> GetUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> ValidateUserCredentials(string userId, string password)
        {
            throw new NotImplementedException();
        }
    }
}