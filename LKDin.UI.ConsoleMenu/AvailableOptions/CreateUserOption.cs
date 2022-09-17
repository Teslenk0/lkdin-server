using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.IBusinessLogic;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class CreateUserOption : UserProtectedConsoleMenuOption
    {
        private readonly IUserService _userService;

        public CreateUserOption(string messageToPrint, IUserService userService) : base(messageToPrint)
        {
            this._userService = userService;
        }

        protected override void PerformExecution()
        {
            UserDTO userDTO = new()
            {
                Id = this.RequestUserId(),
                Name = this.RequestName(),
                Password = this.RequestPassword(),
            };

            this._userService.CreateUser(userDTO);

            this.PrintFinishedExecutionMessage("Se creo el usuario exitosamente");
        }

        private string RequestName()
        {
            Console.Write("Nombre: ");

            string name;

            do
            {
                name = this.CancelableReadLine();

                if (name == null || name.Length < 2 || !name.All(c => char.IsWhiteSpace(c) || char.IsLetter(c)))
                {
                    this.PrintError("Valor incorrecto");
                    Console.Write("Nombre: ");
                }
            }
            while (name == null || name.Length < 2 || !name.All(c => char.IsWhiteSpace(c) || char.IsLetter(c)));

            return name;
        }
    }
}
