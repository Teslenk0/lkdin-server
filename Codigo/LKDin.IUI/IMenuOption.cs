namespace LKDin.IUI;

public interface IMenuOption
{
    public string MessageToPrint { get; set; }

    public int OptionNumber { get; set; }

    public Task Execute();

    public void PrintMenuOptionMessage();
}