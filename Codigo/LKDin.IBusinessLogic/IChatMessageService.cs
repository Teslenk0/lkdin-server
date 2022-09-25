using LKDin.DTOs;

namespace LKDin.IBusinessLogic
{
    public interface IChatMessageService
    {
        public void CreateChatMessage(ChatMessageDTO chatMessageDTO);

        public List<ChatMessageDTO> GetByReceiverId(UserDTO userDTO);

        public List<ChatMessageDTO> GetBySenderId(UserDTO userDTO);

        public void MarkMessagesAsRead(List<string> messagesIds);
    }
}
