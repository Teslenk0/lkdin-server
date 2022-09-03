using LKDin.IUI;

namespace LKDin.UI.ConsoleMenu;

public abstract class ConsoleMenuOption : IMenuOption
{
    public string MessageToPrint { get; set; }

    public int OptionNumber { get; set; }

    public ConsoleMenuOption(string messageToPrint)
    {
        MessageToPrint = messageToPrint;
    }

    public abstract void Execute();

    public void PrintMessage()
    {
        Console.WriteLine(OptionNumber + " - " + MessageToPrint);
    }
}