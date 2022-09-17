using LKDin.Server.Domain;

namespace LKDin.Server.IDataAccess.Repositories
{
    public interface IChatMessageRepository
    {
        public ChatMessage Create(ChatMessage chatMessage);

        public List<ChatMessage> GetBySenderId(string userId, bool includeReadMessages);

        public List<ChatMessage> GetByReceiverId(string userId, bool includeReadMessages);

        public void MarkMessagesAsRead(List<string> messagesIds);
    }
}