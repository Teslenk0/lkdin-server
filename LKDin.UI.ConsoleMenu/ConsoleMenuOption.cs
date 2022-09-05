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
}