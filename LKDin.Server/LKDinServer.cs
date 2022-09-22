using LKDin.Helpers.Configuration;
using LKDin.IUI;
using LKDin.Server.BusinessLogic;
using LKDin.Server.Networking;
using LKDin.Networking;
using LKDin.UI.ConsoleMenu;
using LKDin.UI.ConsoleMenu.AvailableOptions;

namespace LKDin.Server;

public class LKDinServer
{
    private static string UI_THREAD_NAME = "LKDIN_SERVER_UI";

    private static string BG_SOCKET_LISTENER_THREAD_NAME = "BG_SOCKET_LISTENER";

    private static void InitServerUI()
    {
        var userService = new UserService();

        var workProfileService = new WorkProfileService(userService);

        var chatMessageService = new ChatMessageService(userService);

        INetworkingManager serverNetworkingManager = ServerNetworkingManager.Instance;

        var enabledOptions = new List<IMenuOption>()
        {
            new CreateUserOption("Crear Usuario", userService),
            new CreateWorkProfileOption("Crear Perfil de Trabajo", workProfileService),
            new AssignImageToWorkProfile("Asignar Imagen a Perfil de Trabajo", workProfileService),
            new SendChatMessageOption("Enviar Mensaje", chatMessageService),
            new CheckChatMessagesOption("Revisar Mensajes", chatMessageService),
            new SearchWorkProfilesBySkillsOption("Buscar Perfiles por Habilidades", workProfileService),
            new SearchWorkProfilesByDescriptionOption("Buscar Perfiles por Descripción", workProfileService),
            new ShowUserOption("Buscar Perfil por ID", workProfileService),
            new ExitOption("Salir", serverNetworkingManager)
        };

        IUIService uiService = new ConsoleMenuService(enabledOptions);

        uiService.Render();
    }

    private static void InitBackgroundService()
    {
        var networkingManager = ServerNetworkingManager.Instance;

        // Start listening
        networkingManager.InitSocketV4Connection();

        // Start accepting connections
        networkingManager.AcceptSocketConnections(ServerConnectionHandler.HandleConnection);
    }

    public static void Main()
    {
        // Init the background service in a different thread
        Thread backgroundServiceThread = new(InitBackgroundService);

        backgroundServiceThread.Name = BG_SOCKET_LISTENER_THREAD_NAME;

        backgroundServiceThread.Start();

        // Wait a couple seconds to print the message
        Console.WriteLine("Cargando...");
        Thread.Sleep(2000);
        Console.Clear();

        //// Init the UI in a different thread
        Thread serverUIThread = new(InitServerUI);

        serverUIThread.Name = UI_THREAD_NAME;

        serverUIThread.Start();
    }
}

