using LKDin.Server.Domain;

namespace LKDin.Server.IDataAccess.Repositories
{
    public interface IChatMessageRepository
    {
        public ChatMessage Create(ChatMessage chatMessage);

        public List<ChatMessage> GetBySenderId(string userId);

        public List<ChatMessage> GetByReceiverId(string userId);

        public void MarkMessagesAsRead(List<string> messagesIds);

        public void MarkMessageAsRead(string messageId);
    }
}