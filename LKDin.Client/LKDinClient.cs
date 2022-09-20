using LKDin.IUI;
using LKDin.Networking;
using LKDin.Server.BusinessLogic;
using LKDin.Server.Networking;
using LKDin.UI.ConsoleMenu;
using LKDin.UI.ConsoleMenu.AvailableOptions;

namespace LKDin.Client;

public class LKDinClient
{
    public static void Main()
    {
        var networkingManager = ClientNetworkingManager.Instance;

        networkingManager.InitSocketV4Connection();

        var socketClient = networkingManager.GetSocket();

        var networkDataHelper = new NetworkDataHelper(socketClient);

        var userService = new UserClientService(networkDataHelper);

        var enabledOptions = new List<IMenuOption>()
        {
            new CreateUserOption("Crear Usuario", userService),
            new ExitOption("Salir", networkingManager)
        };

        IUIService uiService = new ConsoleMenuService(enabledOptions);

        uiService.Render();
    }
}
