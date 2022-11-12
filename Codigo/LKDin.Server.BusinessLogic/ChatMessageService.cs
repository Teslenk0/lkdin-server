using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.IBusinessLogic;
using LKDin.Server.Domain;
using LKDin.Server.DataAccess.Repositories;
using LKDin.Server.IDataAccess.Repositories;
using LKDin.Logging.Client;
using LKDin.Messaging;

namespace LKDin.Server.BusinessLogic
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        private readonly IUserService _userService;

        private readonly Logger _logger;

        public ChatMessageService(IUserService userService)
        {
            this._chatMessageRepository = new ChatMessageRepository();

            this._userService = userService;

            this._logger = new Logger("server:business-logic:chat-message-service");
        }

        public async Task CreateChatMessage(ChatMessageDTO chatMessageDTO)
        {
            this._logger.Info($"Creando mensaje de chat - Envia:{chatMessageDTO.SenderId} | Recibe:{chatMessageDTO.ReceiverId}");

            await this._userService.ValidateUserCredentials(chatMessageDTO.SenderId, chatMessageDTO.UserPassword);

            var receiver = this._userService.GetUser(chatMessageDTO.ReceiverId);

            if(receiver == null)
            {
                this._logger.Error($"Receptor ID:{chatMessageDTO.ReceiverId} no existe");

                throw new UserDoesNotExistException(chatMessageDTO.ReceiverId);
            }

            var message = ChatMessageDTO.DTOToEntity(chatMessageDTO);

            message.Read = false;

            this._chatMessageRepository.Create(message);

            this._logger.Info($"Se creó el mensaje de chat exitosamente - Envia:{chatMessageDTO.SenderId} | Recibe:{chatMessageDTO.ReceiverId}");
        }

        public async Task<List<ChatMessageDTO>> GetByReceiverId(UserDTO userDTO)
        {
            this._logger.Info($"Obteniendo mensajes por receptor ID:{userDTO.Id}");

            await this._userService.ValidateUserCredentials(userDTO.Id, userDTO.Password);

            var receivedMessages = this._chatMessageRepository.GetByReceiverId(userDTO.Id);
           
            return await AssignUsersToChatMessages(receivedMessages);
        }

        public async Task<List<ChatMessageDTO>> GetBySenderId(UserDTO userDTO)
        {
            this._logger.Info($"Obteniendo mensajes por emisor ID:{userDTO.Id}");

            await this._userService.ValidateUserCredentials(userDTO.Id, userDTO.Password);

            var sentMessages = this._chatMessageRepository.GetBySenderId(userDTO.Id);

            return await AssignUsersToChatMessages(sentMessages);
        }

        public async Task MarkMessagesAsRead(List<string> messagesIds)
        {
            this._logger.Info("Marcando mensajes como leídos");

            this._chatMessageRepository.MarkMessagesAsRead(messagesIds);
        }

        public async Task MarkMessageAsRead(string messageId)
        {
            this._logger.Info($"Marcando mensaje ID:{messageId} como leído");

            this._chatMessageRepository.MarkMessageAsRead(messageId);
        }

        private async Task<List<ChatMessageDTO>> AssignUsersToChatMessages(List<ChatMessage> chatMessages)
        {
            this._logger.Info($"Asignando usuarios a mensajes de chat");

            var result = new List<ChatMessageDTO>();

            var localUsersCache = new Dictionary<string, UserDTO>();

            chatMessages.ForEach(async message =>
            {
                var messageDTO = ChatMessageDTO.EntityToDTO(message);

                if (localUsersCache.ContainsKey(message.SenderId))
                {
                    messageDTO.Sender = localUsersCache[message.SenderId];
                }
                else
                {
                    var user = await _userService.GetUser(message.SenderId);

                    messageDTO.Sender = user;

                    localUsersCache.Add(message.SenderId, user);
                }

                if (localUsersCache.ContainsKey(message.ReceiverId))
                {
                    messageDTO.Receiver = localUsersCache[message.ReceiverId];
                }
                else
                {
                    var user = await _userService.GetUser(message.ReceiverId);

                    messageDTO.Receiver = user;

                    localUsersCache.Add(message.ReceiverId, user);
                }

                result.Add(messageDTO);
            });

            return result;
        }
    }
}