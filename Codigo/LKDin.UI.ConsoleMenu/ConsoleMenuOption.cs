using System.Text;
using LKDin.Exceptions;
using LKDin.IUI;
using LKDin.UI.ConsoleMenu.Extensions;

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

    protected abstract void PerformExecution();

    public void Execute()
    {
        try
        {
            this.PrintHeader(this.MessageToPrint);

            this.PerformExecution();

        }
        catch (AbortCommandExecutionException)
        {
            this.PrintFinishedExecutionMessage("Operación abortada", true);
        }
        catch (Exception e)
        {
            this.PrintError(e.Message);

            this.PrintFinishedExecutionMessage(null, false);
        }
    }

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

        if (message != null)
        {
            Console.Write($"{message}. Presione cualquier tecla para continuar...");
        }
        else
        {
            Console.Write("Presione cualquier tecla para continuar...");
        }

        Console.ReadKey();
    }

    protected string CancelableReadLine()
    {
        var clOffset = Console.CursorLeft;
        var line = string.Empty;
        var buffer = new StringBuilder();
        var key = Console.ReadKey(true);
        while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
        {
            if (key.Key == ConsoleKey.Backspace && Console.CursorLeft - clOffset > 0)
            {
                var cli = Console.CursorLeft - clOffset - 1;
                buffer.Remove(cli, 1);
                Console.CursorLeft = clOffset;
                Console.Write(new string(' ', buffer.Length + 1));
                Console.CursorLeft = clOffset;
                Console.Write(buffer.ToString());
                Console.CursorLeft = cli + clOffset;
                key = Console.ReadKey(true);
            }
            else if (key.Key == ConsoleKey.Delete && Console.CursorLeft - clOffset < buffer.Length)
            {
                var cli = Console.CursorLeft - clOffset;
                buffer.Remove(cli, 1);
                Console.CursorLeft = clOffset;
                Console.Write(new string(' ', buffer.Length + 1));
                Console.CursorLeft = clOffset;
                Console.Write(buffer.ToString());
                Console.CursorLeft = cli + clOffset;
                key = Console.ReadKey(true);
            }
            else if (char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || char.IsWhiteSpace(key.KeyChar))
            {
                var cli = Console.CursorLeft - clOffset;
                buffer.Insert(cli, key.KeyChar);
                Console.CursorLeft = clOffset;
                Console.Write(buffer.ToString());
                Console.CursorLeft = cli + clOffset + 1;
                key = Console.ReadKey(true);
            }
            else if (key.Key == ConsoleKey.LeftArrow && Console.CursorLeft - clOffset > 0)
            {
                Console.CursorLeft--;
                key = Console.ReadKey(true);
            }
            else if (key.Key == ConsoleKey.RightArrow && Console.CursorLeft - clOffset < buffer.Length)
            {
                Console.CursorLeft++;
                key = Console.ReadKey(true);
            }
            else
            {
                key = Console.ReadKey(true);
            }
        }

        if(key.Key == ConsoleKey.Escape)
        {
            Console.WriteLine();
            throw new AbortCommandExecutionException();
        }

        if (key.Key == ConsoleKey.Enter)
        {
            Console.WriteLine();
            line = buffer.ToString();
            return line;
        }
        return line;
    }

    protected void PrintDataInTable<T>(List<T> data, string[] columnNames, params Func<T, object>[] valueSelectors)
    {
        var customTable = data.ToStringTable(columnNames, valueSelectors);
        this.PrintInfoDiv();
        Console.WriteLine(customTable);
    }
}