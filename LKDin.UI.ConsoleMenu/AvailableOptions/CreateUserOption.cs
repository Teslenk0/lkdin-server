using LKDin.DTOs;
using LKDin.IBusinessLogic;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class CreateUserOption : ConsoleMenuOption
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
                this.PrintHeader("Crear Usuario");
    
                UserDTO userDTO = new()
                {
                    Id = this.RequestId(),
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

        private string RequestPassword()
        {
            Console.Write("Contraseña: ");

            string password;

            do
            {
                password = Console.ReadLine();

                if (password == null || password.Length < 5)
                {
                    this.PrintError("Valor incorrecto (al menos 5 caracteres)");
                    Console.Write("Contraseña: ");
                }
            }
            while (password == null || password.Length < 5);

            return password;
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

        private string RequestId()
        {
            Console.Write("ID (CI, DNI): ");

            string id;

            do
            {
                id = Console.ReadLine();

                if (id == null || id.Length < 1 || !id.All(char.IsLetterOrDigit))
                {
                    this.PrintError("Valor incorrecto (solo caracteres alfanumericos)");
                    Console.Write("ID (CI, DNI): ");
                }
            }
            while (id == null || id.Length < 1 || !id.All(char.IsLetterOrDigit));

            return id;
        }
    }
}
