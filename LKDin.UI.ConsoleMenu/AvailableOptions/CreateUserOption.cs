using LKDin.DTOs;
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

        public override void Execute()
        {
            try
            {
                this.PrintHeader(this.MessageToPrint);
    
                UserDTO userDTO = new()
                {
                    Id = this.RequestUserId(),
                    Name = this.RequestName(),
                    Password = this.RequestPassword(),
                };

                this._userService.CreateUser(userDTO);

                this.PrintFinishedExecutionMessage("Se creo el usuario exitosamente");
            }
            catch (Exception e)
            {
                this.PrintError(e.Message);

                this.PrintFinishedExecutionMessage(null, false);
            }
        }

        private string RequestName()
        {
            Console.Write("Nombre: ");

            string name;

            do
            {
                name = Console.ReadLine();

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
