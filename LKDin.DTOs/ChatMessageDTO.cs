using LKDin.Server.Domain;

namespace LKDin.DTOs
{
    public class ChatMessageDTO
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public UserDTO Sender { get; set; }

        public string SenderId { get; set; }

        public UserDTO Receiver { get; set; }

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

            if (chatMessage.Sender != null) 
            {
                chatMessageDTO.Sender = UserDTO.EntityToDTO(chatMessage.Sender);
            }

            if (chatMessage.Receiver != null)
            {
                chatMessageDTO.Receiver = UserDTO.EntityToDTO(chatMessage.Receiver);
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
                chatMessage.Id = Guid.Parse(chatMessageDTO.Id);
            }

            if (chatMessageDTO.Sender != null)
            {
                chatMessage.Sender = UserDTO.DTOToEntity(chatMessageDTO.Sender);
            }

            if (chatMessageDTO.Receiver != null)
            {
                chatMessage.Receiver = UserDTO.DTOToEntity(chatMessageDTO.Receiver);
            }

            return chatMessage;
        }
    }
}