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
    private static async Task InitServerUI()
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

        await uiService.Render();
    }

    public static async Task Main()
    {
        Console.WriteLine("Iniciando...");

        var networkingManager = ServerNetworkingManager.Instance;

        var connected = await networkingManager.InitTCPConnection();

        if (connected)
        {
            Console.WriteLine("Información de la aplicación en: {0}", ConfigManager.GetAppDataBasePath());

            // We don't need to await this.
            networkingManager.AcceptTCPConnections(ServerConnectionHandler.HandleConnection);

            Console.WriteLine("Cargando...");

            await Task.Delay(3000);

            Console.Clear();

            await InitServerUI();
        }
        
    }
}

