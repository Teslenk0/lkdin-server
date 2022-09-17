using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.DataAccess.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        public ChatMessage Create(ChatMessage chatMessage)
        {
            chatMessage.Id = Guid.NewGuid().ToString();

            LKDinDataManager.AddDataToStore<ChatMessage>(chatMessage);

            return chatMessage;
        }

        public List<ChatMessage> GetByReceiverId(string userId, bool includeReadMessages)
        {
            var messages = LKDinDataManager.ChatMessages.Where(chatMessage => chatMessage.ReceiverId.Equals(userId));

            if (!includeReadMessages)
            {
                return messages.Where(chatMessage => chatMessage.Read == false).ToList();
            }

            return messages.ToList();
        }

        public List<ChatMessage> GetBySenderId(string userId, bool includeReadMessages)
        {
            var messages = LKDinDataManager.ChatMessages.Where(chatMessage => chatMessage.SenderId.Equals(userId));

            if (!includeReadMessages)
            {
                return messages.Where(chatMessage => chatMessage.Read == false).ToList();
            }

            return messages.ToList();
        }
    }
}
