using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.IBusinessLogic;
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

        public List<ChatMessageDTO> GetByReceiverId(string userId, bool includeReadMessages)
        {
            throw new NotImplementedException();
        }

        public List<ChatMessageDTO> GetBySenderId(string userId, bool includeReadMessages)
        {
            throw new NotImplementedException();
        }
    }
}