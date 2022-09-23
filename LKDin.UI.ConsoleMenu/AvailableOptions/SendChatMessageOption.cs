using LKDin.DTOs;
using LKDin.IBusinessLogic;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class SendChatMessageOption : UserProtectedConsoleMenuOption
    {
        private readonly IChatMessageService _chatMessageService;

        public SendChatMessageOption(string messageToPrint, IChatMessageService chatMessageService) : base(messageToPrint)
        {
            this._chatMessageService = chatMessageService;
        }

        protected override void PerformExecution()
        {
            var senderId = this.RequestUserId(" Emisor ");

            ChatMessageDTO chatMessageDTO = new()
            {
                UserId = senderId,
                SenderId = senderId,
                UserPassword = this.RequestPassword(),
                ReceiverId = this.RequestUserId(" Receptor "),
                Content = this.RequestMessageContent(),
            };

            this._chatMessageService.CreateChatMessage(chatMessageDTO);

            this.PrintFinishedExecutionMessage("Se envió el mensaje exitosamente");
        }

        private string RequestMessageContent()
        {
            Console.Write("Mensaje: ");

            string content;

            do
            {
                content = this.CancelableReadLine();

                if (string.IsNullOrWhiteSpace(content) || content.Length < 2)
                {
                    this.PrintError("El mensaje no puede estar vacio");
                    Console.Write("Mensaje: ");
                }
            }
            while (string.IsNullOrWhiteSpace(content) || content.Length < 2);

            return content;
        }
    }
}
