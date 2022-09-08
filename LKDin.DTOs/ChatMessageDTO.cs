using LKDin.Server.Domain;

namespace LKDin.DTOs
{
    public class ChatMessageDTO
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public bool Read { get; set; }

        public static ChatMessageDTO EntityToDTO(ChatMessage chatMessage)
        {
            var chatMessageDTO = new ChatMessageDTO()
            {
                Id = chatMessage.Id.ToString(),
                Content = chatMessage.Content,
                SenderId = chatMessage.SenderId,
                ReceiverId = chatMessage.ReceiverId,
                Read = chatMessage.Read
            };

            return chatMessageDTO;
        }

        public static ChatMessage DTOToEntity(ChatMessageDTO chatMessageDTO)
        {
            var chatMessage = new ChatMessage()
            {
                Content = chatMessageDTO.Content,
                SenderId = chatMessageDTO.SenderId,
                ReceiverId = chatMessageDTO.ReceiverId,
                Read = chatMessageDTO.Read
            };

            if(chatMessageDTO.Id != null)
            {
                chatMessage.Id = Guid.Parse(chatMessageDTO.Id);
            }

            return chatMessage;
        }
    }
}