using LKDin.Server.Domain;

namespace LKDin.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public WorkProfileDTO WorkProfile { get; set; }

        public List<ChatMessageDTO> ReceivedMessages { get; set; }

        public List<ChatMessageDTO> SentMessages { get; set; }

        public static UserDTO EntityToDTO(User user)
        {
            var userDTO = new UserDTO()
            {
                Id = user.Id,
                Name = user.Name,
                SentMessages = new List<ChatMessageDTO>(),
                ReceivedMessages = new List<ChatMessageDTO>(),
                Password = user.Password,
            };

            if(user.WorkProfile != null)
            {
                userDTO.WorkProfile = WorkProfileDTO.EntityToDTO(user.WorkProfile);
            }

            if (user.SentMessages != null)
            {
                user.SentMessages.ToList().ForEach(chatMessage =>
                {
                    userDTO.SentMessages.Add(ChatMessageDTO.EntityToDTO(chatMessage));
                });
            }

            if (user.ReceivedMessages != null)
            {
                user.ReceivedMessages.ToList().ForEach(chatMessage =>
                {
                    userDTO.ReceivedMessages.Add(ChatMessageDTO.EntityToDTO(chatMessage));
                });
            }

            return userDTO;
        }

        public static User DTOToEntity(UserDTO userDTO)
        {
            var user = new User()
            {
                Id = userDTO.Id,
                Name = userDTO.Name,
                SentMessages = new List<ChatMessage>(),
                ReceivedMessages = new List<ChatMessage>(),
                Password = userDTO.Password,
            };


            if (user.WorkProfile != null)
            {
                user.WorkProfile = WorkProfileDTO.DTOToEntity(userDTO.WorkProfile);
            }

            if (userDTO.SentMessages != null)
            {
                userDTO.SentMessages.ForEach(chatMessage =>
                {
                    user.SentMessages.Add(ChatMessageDTO.DTOToEntity(chatMessage));
                });
            }

            if (userDTO.ReceivedMessages != null)
            {
                userDTO.ReceivedMessages.ForEach(chatMessage =>
                {
                    user.ReceivedMessages.Add(ChatMessageDTO.DTOToEntity(chatMessage));
                });
            }

            return user;
        }
    }
}