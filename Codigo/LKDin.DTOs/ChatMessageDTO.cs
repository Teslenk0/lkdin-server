using LKDin.Server.Domain;

namespace LKDin.DTOs
{
    public class ChatMessageDTO : ProtectedDTO
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public bool Read { get; set; }

        public UserDTO Receiver { get; set; }

        public UserDTO Sender { get; set; }

        public long? SentAt { get; set; }

        public long? ReadAt { get; set; }

        public static ChatMessageDTO EntityToDTO(ChatMessage chatMessage)
        {
            var chatMessageDTO = new ChatMessageDTO()
            {
                Id = chatMessage.Id.ToString(),
                Content = chatMessage.Content,
                SenderId = chatMessage.SenderId,
                ReceiverId = chatMessage.ReceiverId,
                Read = chatMessage.Read,
                SentAt = chatMessage.SentAt,
            };

            if(chatMessage.ReadAt != null)
            {
                chatMessageDTO.ReadAt = chatMessage.ReadAt;
            }

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
                chatMessage.Id = chatMessageDTO.Id;
            }

            if (chatMessageDTO.ReadAt != null)
            {
                chatMessage.ReadAt = chatMessageDTO.ReadAt;
            }

            if (chatMessageDTO.SentAt != null)
            {
                chatMessage.SentAt = (long)chatMessageDTO.SentAt;
            }

            return chatMessage;
        }
    }
}