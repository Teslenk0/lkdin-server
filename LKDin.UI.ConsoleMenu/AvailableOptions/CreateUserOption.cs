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
            Console.WriteLine("---------------------");
            Console.WriteLine("     Crear Usuario   ");
            Console.WriteLine("---------------------");

            UserDTO userDTO = new()
            {
                Id = this.RequestId(),
                Name = this.RequestName(),
                Password = this.RequestPassword(),
            };

            this._userService.CreateUser(userDTO);
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
                    Console.WriteLine("==========================");
                    Console.WriteLine("ERROR: Valor incorrecto (al menos 5 caracteres)");
                    Console.WriteLine("==========================");
                    Console.Write("Contraseña: ");
                }
            }
            while (password == null || password.Length < 5);

            return password;
        }

        private string RequestName()
        {
            Console.Write("Nombre (opcional): ");

            string name = Console.ReadLine();

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
                    Console.WriteLine("==========================");
                    Console.WriteLine("ERROR: Valor incorrecto (solo caracteres alfanumericos)");
                    Console.WriteLine("==========================");
                    Console.Write("ID (CI, DNI): ");
                }
            }
            while (id == null || id.Length < 1 || !id.All(char.IsLetterOrDigit));

            return id;
        }
    }
}
