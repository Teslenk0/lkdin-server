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

        public void CreateChatMessage(ChatMessageDTO chatMessageDTO)
        {
            var serializedChatMessage = SerializationManager.Serialize<ChatMessageDTO>(chatMessageDTO);

            _networkDataHelper.SendMessage(serializedChatMessage, AvailableOperation.SEND_CHAT_MESSAGE);

            _networkDataHelper.ReceiveMessage();
        }

        public List<ChatMessageDTO> GetByReceiverId(UserDTO userDTO)
        {
            var serializedUser = SerializationManager.Serialize<UserDTO>(userDTO);

            _networkDataHelper.SendMessage(serializedUser, AvailableOperation.CHECK_CHAT_MESSAGES_BY_RECEIVER_ID);

            var data = _networkDataHelper.ReceiveMessage();

            var messagePayload = data[NetworkDataHelper.MSG_NAME];

            return SerializationManager.DeserializeList<List<ChatMessageDTO>>(messagePayload);
        }

        public List<ChatMessageDTO> GetBySenderId(UserDTO userDTO)
        {
            var serializedUser = SerializationManager.Serialize<UserDTO>(userDTO);

            _networkDataHelper.SendMessage(serializedUser, AvailableOperation.CHECK_CHAT_MESSAGES_BY_SENDER_ID);

            var data = _networkDataHelper.ReceiveMessage();

            var messagePayload = data[NetworkDataHelper.MSG_NAME];

            return SerializationManager.DeserializeList<List<ChatMessageDTO>>(messagePayload);
        }

        public void MarkMessagesAsRead(List<string> messagesIds)
        {
            foreach(var messageId in messagesIds)
            {
                _networkDataHelper.SendMessage(messageId, AvailableOperation.MARK_MESSAGE_AS_READ);

                _networkDataHelper.ReceiveMessage();
            }
        }
    }
}