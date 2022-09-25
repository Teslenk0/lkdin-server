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
            new DownloadWorkProfileImageOption("Descargar Imagen de Perfil de Trabajo", workProfileService),
            new SendChatMessageOption("Enviar Mensaje", chatMessageService),
            new CheckChatMessagesOption("Revisar Mensajes", chatMessageService),
            new SearchWorkProfilesBySkillsOption("Buscar Perfiles por Habilidades", workProfileService),
            new SearchWorkProfilesByDescriptionOption("Buscar Perfiles por Descripción", workProfileService),
            new ShowUserOption("Buscar Perfil por ID", workProfileService),
            new ExitOption("Salir", serverNetworkingManager)
        };

        IUIService uiService = new ConsoleMenuService(enabledOptions, true);

        uiService.Render();
    }

    public static void Main()
    {
        Console.WriteLine("Iniciando...");

        var networkingManager = ServerNetworkingManager.Instance;

        var connected = networkingManager.InitSocketV4Connection();

        if (connected)
        {
            Console.WriteLine("Información de la aplicación en: {0}", ConfigManager.GetAppDataBasePath());

            Thread backgroundServiceThread = new(() => networkingManager.AcceptSocketConnections(ServerConnectionHandler.HandleConnection))
            {
                Name = BG_SOCKET_LISTENER_THREAD_NAME
            };

            backgroundServiceThread.Start();

            Console.WriteLine("Cargando...");
            Thread.Sleep(3000);
            Console.Clear();

            //// Init the UI in a different thread
            Thread serverUIThread = new(InitServerUI);

            serverUIThread.Name = UI_THREAD_NAME;

            serverUIThread.Start();
        }
        
    }
}

