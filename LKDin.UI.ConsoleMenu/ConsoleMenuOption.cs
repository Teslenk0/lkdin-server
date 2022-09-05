using LKDin.IUI;

namespace LKDin.UI.ConsoleMenu;

public abstract class ConsoleMenuOption : IMenuOption
{
    private const int DIV_SIZE = 50;

    private const char INFO_DIV_CHAR = '-';

    private const char ERROR_DIV_CHAR = '=';

    public string MessageToPrint { get; set; }

    public int OptionNumber { get; set; }

    public ConsoleMenuOption(string messageToPrint)
    {
        MessageToPrint = messageToPrint;
    }

    public abstract void Execute();

    public void PrintMenuOptionMessage()
    {
        Console.WriteLine(OptionNumber + " - " + MessageToPrint);
    }

    private void PrintDiv(char characterToPrint)
    {
        for (int i = 0; i < DIV_SIZE - 1; i++)
        {
            Console.Write(characterToPrint);
        }
        Console.WriteLine(characterToPrint);
    }

    protected void PrintInfoDiv()
    {
        this.PrintDiv(INFO_DIV_CHAR);
    }

    protected void PrintError(string errorMessage)
    {
        Console.WriteLine();

        this.PrintDiv(ERROR_DIV_CHAR);

        Console.WriteLine($"ERROR: {errorMessage}");

        this.PrintDiv(ERROR_DIV_CHAR);
    }

    protected void PrintHeader(string headerMessage)
    {
        this.PrintInfoDiv();

        var spacesLength = (DIV_SIZE - headerMessage.Length) / 2;

        for (int i = 0; i < spacesLength - 1; i++)
        {
            Console.Write(" ");
        }

        Console.WriteLine(headerMessage);

        this.PrintInfoDiv();
    }

    protected void PrintFinishedExecutionMessage(string? message, bool printDiv = true)
    {
        if (printDiv)
        {
            this.PrintInfoDiv();
        }

        if(message != null)
        {
            Console.Write($"{message}. Presione cualquier tecla para continuar...");
        } else
        {
            Console.Write("Presione cualquier tecla para continuar...");
        }
        
        Console.ReadLine();
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
                this.PrintError("Valor incorrecto (solo caracteres alfanumericos)");
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
                this.PrintError("Valor incorrecto (al menos 5 caracteres)");
                Console.Write("Contraseña: ");
            }
        }
        while (password == null || password.Length < 5);

        return password;
    }
}