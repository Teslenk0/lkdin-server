using LKDin.DTOs;
using LKDin.Helpers;
using LKDin.IBusinessLogic;
using LKDin.Server.Domain;
using LKDin.UI.ConsoleMenu.Extensions;

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

            this.PrintInfoDiv();

            var messagesSent = this._chatMessageService.GetBySenderId(userDTO, includeReadMessages);

            var messagesReceived = this._chatMessageService.GetByReceiverId(userDTO, includeReadMessages);

            this.PrintFinishedExecutionMessage(null);
        }

        private bool RequestIncludeReadMessages()
        {
            string includeReadMessages = "";

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

            if (includeReadMessages.Equals('s'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void PrintResultsInTable(List<WorkProfileDTO> results)
        {
            var columnNames = new[]
            {
                "ID Usuario",
                "Nombre",
                "Descripcion",
                "Skills"
            };

            this.PrintDataInTable<WorkProfileDTO>(results, columnNames,
                wp => wp.User.Id,
                wp => wp.User.Name,
                wp => wp.Description,
                wp => String.Join<string>(',', wp.Skills.Select(s => s.Name).ToArray()));

        }
    }
}
