namespace LKDin.IUI;

public interface IMenuOption
{
    public string MessageToPrint { get; set; }

    public int OptionNumber { get; set; }

    public void Execute();

    public void PrintMessage();
}