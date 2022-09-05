using LKDin.IUI;
using LKDin.Server.BusinessLogic;
using LKDin.UI.ConsoleMenu;
using LKDin.UI.ConsoleMenu.AvailableOptions;

namespace LKDin.Server;

public class Program
{
    static void Main(string[] args)
    {
        var enabledOptions = new List<IMenuOption>()
        {
            new CreateUserOption("Crear nuevo usuario", new UserService()),
            new ExitOption("Salir")
        };

        IUIService uiService = new ConsoleMenuService(enabledOptions);

        Thread menuThread = new(uiService.Render);

        menuThread.Start();
    }
}

