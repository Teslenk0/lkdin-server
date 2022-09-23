using LKDin.Client.BusinessLogic;
using LKDin.Client.Networking;
using LKDin.IUI;
using LKDin.Networking;
using LKDin.UI.ConsoleMenu;
using LKDin.UI.ConsoleMenu.AvailableOptions;

namespace LKDin.Client;

public class LKDinClient
{
    public static void Main()
    {
        var networkingManager = ClientNetworkingManager.Instance;

        var connected = networkingManager.InitSocketV4Connection();

        var exitOption = new ExitOption("Salir", networkingManager);

        if (connected)
        {
            var socketClient = networkingManager.GetSocket();

            new Thread(() => networkingManager.ValidateConnectionOrShutDown(exitOption.Execute)).Start();

            var networkDataHelper = new NetworkDataHelper(socketClient);

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

            uiService.Render();
        } else
        {
            exitOption.Execute();
        }

        
    }
}
