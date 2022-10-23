using LKDin.DTOs;
using LKDin.Helpers.Serialization;
using LKDin.IBusinessLogic;
using LKDin.Networking;

namespace LKDin.Client.BusinessLogic
{
    public class ChatMessageClientService : IChatMessageService
    {
        private readonly NetworkDataHelper _networkDataHelper;

        public ChatMessageClientService(NetworkDataHelper networkDataHelper)
        {
            _networkDataHelper = networkDataHelper;
        }

        public async Task CreateChatMessage(ChatMessageDTO chatMessageDTO)
        {
            var serializedChatMessage = SerializationManager.Serialize<ChatMessageDTO>(chatMessageDTO);

            await _networkDataHelper.SendMessage(serializedChatMessage, AvailableOperation.SEND_CHAT_MESSAGE);

            await _networkDataHelper.ReceiveMessage();
        }

        public async Task<List<ChatMessageDTO>> GetByReceiverId(UserDTO userDTO)
        {
            var serializedUser = SerializationManager.Serialize<UserDTO>(userDTO);

            await _networkDataHelper.SendMessage(serializedUser, AvailableOperation.CHECK_CHAT_MESSAGES_BY_RECEIVER_ID);

            var data = await _networkDataHelper.ReceiveMessage();

            var messagePayload = data[Protocol.MSG_NAME];

            return SerializationManager.DeserializeList<List<ChatMessageDTO>>(messagePayload);
        }

        public async Task<List<ChatMessageDTO>> GetBySenderId(UserDTO userDTO)
        {
            var serializedUser = SerializationManager.Serialize<UserDTO>(userDTO);

            await _networkDataHelper.SendMessage(serializedUser, AvailableOperation.CHECK_CHAT_MESSAGES_BY_SENDER_ID);

            var data = await _networkDataHelper.ReceiveMessage();

            var messagePayload = data[Protocol.MSG_NAME];

            return SerializationManager.DeserializeList<List<ChatMessageDTO>>(messagePayload);
        }

        public async Task MarkMessagesAsRead(List<string> messagesIds)
        {
            foreach(var messageId in messagesIds)
            {
                await _networkDataHelper.SendMessage(messageId, AvailableOperation.MARK_MESSAGE_AS_READ);

                await _networkDataHelper.ReceiveMessage();
            }   
        }
    }
}