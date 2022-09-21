using LKDin.IUI;

namespace LKDin.UI.ConsoleMenu.AvailableOptions;

public abstract class UserProtectedConsoleMenuOption : ConsoleMenuOption
{
    public UserProtectedConsoleMenuOption(string messageToPrint) : base(messageToPrint)
    {
        MessageToPrint = messageToPrint;
    }

    protected string RequestUserId(string userAlias = " ")
    {
        Console.Write("ID Usuario{0}(CI, DNI): ", userAlias);

        string id;

        do
        {
            id = this.CancelableReadLine();

            if (id == null || id.Length < 1 || !id.All(char.IsLetterOrDigit))
            {
                PrintError("Valor incorrecto (solo caracteres alfanumericos)");
                Console.Write("ID Usuario{0}(CI, DNI): ", userAlias);
            }
        }
        while (id == null || id.Length < 1 || !id.All(char.IsLetterOrDigit));

        return id;
    }

    protected string RequestPassword()
    {
        Console.Write("Contrasena: ");

        string password;

        do
        {
            password = this.CancelableReadLine();

            if (password == null || password.Length < 5)
            {
                PrintError("Valor incorrecto (al menos 5 caracteres)");
                Console.Write("Contrasena: ");
            }
        }
        while (password == null || password.Length < 5);

        return password;
    }
}