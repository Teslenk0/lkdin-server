using LKDin.DTOs;

namespace LKDin.IBusinessLogic
{
    public interface IChatMessageService
    {
        public void CreateChatMessage(ChatMessageDTO chatMessageDTO);

        public List<ChatMessageDTO> GetByReceiverId(UserDTO userDTO, bool includeReadMessages);

        public List<ChatMessageDTO> GetBySenderId(UserDTO userDTO, bool includeReadMessages);

        public void MarkMessagesAsRead(List<string> messagesIds);
    }
}
