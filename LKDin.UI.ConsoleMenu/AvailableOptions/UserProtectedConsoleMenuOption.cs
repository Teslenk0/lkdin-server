using LKDin.IUI;

namespace LKDin.UI.ConsoleMenu.AvailableOptions;

public abstract class UserProtectedConsoleMenuOption : ConsoleMenuOption
{
    public UserProtectedConsoleMenuOption(string messageToPrint) : base(messageToPrint)
    {
        MessageToPrint = messageToPrint;
    }

    protected string RequestUserId()
    {
        Console.Write("ID (CI, DNI): ");

        string id;

        do
        {
            id = Console.ReadLine();

            if (id == null || id.Length < 1 || !id.All(char.IsLetterOrDigit))
            {
                PrintError("Valor incorrecto (solo caracteres alfanumericos)");
                Console.Write("ID (CI, DNI): ");
            }
        }
        while (id == null || id.Length < 1 || !id.All(char.IsLetterOrDigit));

        return id;
    }

    protected string RequestPassword()
    {
        Console.Write("Contraseña: ");

        string password;

        do
        {
            password = Console.ReadLine();

            if (password == null || password.Length < 5)
            {
                PrintError("Valor incorrecto (al menos 5 caracteres)");
                Console.Write("Contraseña: ");
            }
        }
        while (password == null || password.Length < 5);

        return password;
    }
}