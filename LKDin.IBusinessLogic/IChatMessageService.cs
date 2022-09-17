using LKDin.DTOs;

namespace LKDin.IBusinessLogic
{
    public interface IChatMessageService
    {
        public void CreateChatMessage(ChatMessageDTO chatMessageDTO);

        public List<ChatMessageDTO> GetByReceiverId(string userId, bool includeReadMessages);

        public List<ChatMessageDTO> GetBySenderId(string userId, bool includeReadMessages);
    }
}
