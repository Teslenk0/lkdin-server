using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.IBusinessLogic;
using LKDin.Server.Domain;
using LKDin.Server.DataAccess.Repositories;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.BusinessLogic
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        private readonly IUserService _userService;

        public ChatMessageService(IUserService userService)
        {
            this._chatMessageRepository = new ChatMessageRepository();

            this._userService = userService;
        }

        public void CreateChatMessage(ChatMessageDTO chatMessageDTO)
        {
            this._userService.ValidateUserCredentials(chatMessageDTO.SenderId, chatMessageDTO.UserPassword);

            var receiver = this._userService.GetUser(chatMessageDTO.ReceiverId);

            if(receiver == null)
            {
                throw new UserDoesNotExistException(chatMessageDTO.ReceiverId);
            }

            var message = ChatMessageDTO.DTOToEntity(chatMessageDTO);

            message.Read = false;

            this._chatMessageRepository.Create(message);
        }

        public List<ChatMessageDTO> GetByReceiverId(UserDTO userDTO, bool includeReadMessages)
        {
            this._userService.ValidateUserCredentials(userDTO.Id, userDTO.Password);

            var receivedMessages = this._chatMessageRepository.GetByReceiverId(userDTO.Id, includeReadMessages);
            
            return this.AssignUsersToChatMessages(receivedMessages);
        }

        public List<ChatMessageDTO> GetBySenderId(UserDTO userDTO, bool includeReadMessages)
        {
            this._userService.ValidateUserCredentials(userDTO.Id, userDTO.Password);

            var sentMessages = this._chatMessageRepository.GetBySenderId(userDTO.Id, includeReadMessages);

            return this.AssignUsersToChatMessages(sentMessages);
        }

        private List<ChatMessageDTO> AssignUsersToChatMessages(List<ChatMessage> chatMessages)
        {
            var result = new List<ChatMessageDTO>();

            var localUsersCache = new Dictionary<string, UserDTO>();

            chatMessages.ForEach(message =>
            {
                var messageDTO = ChatMessageDTO.EntityToDTO(message);

                if (localUsersCache.ContainsKey(message.SenderId))
                {
                    messageDTO.Sender = localUsersCache[message.SenderId];
                }
                else
                {
                    var user = this._userService.GetUser(message.SenderId);

                    messageDTO.Sender = user;

                    localUsersCache.Add(message.SenderId, user);
                }

                if (localUsersCache.ContainsKey(message.ReceiverId))
                {
                    messageDTO.Receiver = localUsersCache[message.ReceiverId];
                }
                else
                {
                    var user = this._userService.GetUser(message.ReceiverId);

                    messageDTO.Receiver = user;

                    localUsersCache.Add(message.ReceiverId, user);
                }

                result.Add(messageDTO);
            });

            return result;
        }
    }
}