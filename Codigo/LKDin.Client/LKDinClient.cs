using LKDin.Client.BusinessLogic;
using LKDin.Client.Networking;
using LKDin.Helpers.Configuration;
using LKDin.IUI;
using LKDin.Networking;
using LKDin.UI.ConsoleMenu;
using LKDin.UI.ConsoleMenu.AvailableOptions;

namespace LKDin.Client;

public class LKDinClient
{
    public static async Task Main()
    {
        Console.WriteLine("Iniciando...");

        var networkingManager = ClientNetworkingManager.Instance;

        var connected = await networkingManager.InitTCPConnection();

        var exitOption = new ExitOption("Salir", networkingManager);

        if (connected)
        {
            Console.WriteLine("Información de la aplicación cliente en: {0}", ConfigManager.GetAppDataBasePath());

            Console.WriteLine("Cargando...");

            await Task.Delay(3000);

            Console.Clear();

            var tcpClient = networkingManager.GetClient();

            // We don't need to await this since it's ok for it to run in the background
            networkingManager.ValidateConnectionOrShutDown(exitOption.Execute);

            var networkDataHelper = new NetworkDataHelper(tcpClient);

            var userService = new UserClientService(networkDataHelper);

            var workProfileService = new WorkProfileClientService(networkDataHelper);

            var chatMessageService = new ChatMessageClientService(networkDataHelper);

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
                exitOption
            };

            IUIService uiService = new ConsoleMenuService(enabledOptions, false);

            await uiService.Render();
        } else
        {
            await exitOption.Execute();
        }

        
    }
}
