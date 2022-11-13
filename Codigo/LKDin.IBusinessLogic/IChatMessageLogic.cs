using LKDin.DTOs;

namespace LKDin.IBusinessLogic
{
    public interface IChatMessageLogic
    {
        public Task CreateChatMessage(ChatMessageDTO chatMessageDTO);

        public Task<List<ChatMessageDTO>> GetByReceiverId(UserDTO userDTO);

        public Task<List<ChatMessageDTO>> GetBySenderId(UserDTO userDTO);

        public Task MarkMessagesAsRead(List<string> messagesIds);
    }
}
