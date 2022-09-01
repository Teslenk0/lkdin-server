namespace LKDin.Server.UI;

public class MenuService : IUiService
{
    private readonly List<MenuOption> MenuOptions;

    public MenuService()
    {
        this.MenuOptions = new List<MenuOption>();
    }

    public void Render()
    {
        Console.WriteLine("---------------------");
        Console.WriteLine("    LKDin (v1.0.0)   ");
        Console.WriteLine("---------------------");

        foreach(MenuOption menuOption in MenuOptions)
        {

        }
    }
}