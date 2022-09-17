using LKDin.DTOs;
using LKDin.IBusinessLogic;
using LKDin.Server.Domain;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class CheckChatMessagesOption : UserProtectedConsoleMenuOption
    {
        private readonly IChatMessageService _chatMessageService;

        public CheckChatMessagesOption(string messageToPrint, IChatMessageService chatMessageService) : base(messageToPrint)
        {
            this._chatMessageService = chatMessageService;
        }

        protected override void PerformExecution()
        {
            UserDTO userDTO = new()
            {
                Id = this.RequestUserId(),
                Password = this.RequestPassword()
            };

            var includeReadMessages = this.RequestIncludeReadMessages();

            var messagesSent = this._chatMessageService.GetBySenderId(userDTO, includeReadMessages);

            var messagesReceived = this._chatMessageService.GetByReceiverId(userDTO, includeReadMessages);

            this.PrintMessages(messagesReceived, messagesSent);

            var receivedMessagesIds = messagesReceived.Select(message => message.Id).ToList();

            // Chat messages are marked as read after pressing a key requested by PrintFinishedExecutionMessage
            this._chatMessageService.MarkMessagesAsRead(receivedMessagesIds);
        }

        private bool RequestIncludeReadMessages()
        {
            string includeReadMessages;

            string[] availableOptions = new string[] { "s", "n" };

            Console.Write("Incluir mensajes leidos? (S/N): ");

            do
            {
                includeReadMessages = this.CancelableReadLine().ToLower();

                if (!availableOptions.Contains(includeReadMessages))
                {
                    this.PrintError("Valor incorrecto");
                    Console.Write("Incluir mensajes leidos? (S/N): ");
                }

            } while (!availableOptions.Contains(includeReadMessages));

            if (includeReadMessages.Equals("s"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void PrintMessages(List<ChatMessageDTO> receivedMessages, List<ChatMessageDTO> sentMessages)
        {
            // Dictionary of conversations, they key is the receiver and the value a list with messages;
            var conversationsDictionary = new Dictionary<string, List<ChatMessageDTO>>();

            var conversationsHeaders = new Dictionary<string, string>();

            receivedMessages.ForEach(message =>
            {
                if (!conversationsDictionary.ContainsKey(message.SenderId))
                {
                    conversationsDictionary[message.SenderId] = new List<ChatMessageDTO>();
                }

                if (!conversationsHeaders.ContainsKey(message.SenderId))
                {
                    conversationsHeaders[message.SenderId] = $"Conversación con {message.Sender.Name} - {message.SenderId}";
                }

                conversationsDictionary[message.SenderId].Add(message);
            });

            sentMessages.ForEach(message =>
            {
                if (!conversationsDictionary.ContainsKey(message.ReceiverId))
                {
                    conversationsDictionary[message.ReceiverId] = new List<ChatMessageDTO>();
                }

                if (!conversationsHeaders.ContainsKey(message.ReceiverId))
                {
                    conversationsHeaders[message.ReceiverId] = $"Conversación con {message.Receiver.Name} - {message.ReceiverId}";
                }

                conversationsDictionary[message.ReceiverId].Add(message);
            });

            conversationsDictionary.Keys.ToList().ForEach(conversationKey =>
            {
                var orderedListOfMessages = conversationsDictionary[conversationKey].OrderBy(chatMessage => chatMessage.SentAt).ToList();

                this.PrintInfoDiv();
                Console.WriteLine();

                Console.WriteLine(conversationsHeaders[conversationKey]);

                this.PrintConversationTable(orderedListOfMessages);
            });

            this.PrintFinishedExecutionMessage(null);
        }

        private void PrintConversationTable(List<ChatMessageDTO> results)
        {
            var columnNames = new[]
            {
                "Fecha",
                "Enviado Por",
                "Mensaje",
                "Visto?"
            };

            this.PrintDataInTable<ChatMessageDTO>(results, columnNames,
                cm => DateTimeOffset.FromUnixTimeSeconds((long)cm.SentAt).ToString("g"),
                cm => cm.Sender.Name,
                cm => cm.Content,
                cm => cm.Read ? "SI" : "NO");

        }
    }
}
