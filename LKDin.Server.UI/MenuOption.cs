namespace LKDin.Server.UI;

public abstract class MenuOption
{
    private string MessageToPrint { get; set; }

    public int OptionNumber { get; set; }

    public MenuOption(string messageToPrint)
    {
        this.MessageToPrint = messageToPrint;
    }

    public abstract void Execute();

    public void PrintMessage()
    {
        Console.WriteLine(this.OptionNumber + "-" + this.MessageToPrint);
    }
}