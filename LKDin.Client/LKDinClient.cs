using LKDin.Client.BusinessLogic;
using LKDin.Client.Networking;
using LKDin.IBusinessLogic;
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

        networkingManager.InitSocketV4Connection();

        var socketClient = networkingManager.GetSocket();

        var networkDataHelper = new NetworkDataHelper(socketClient);

        var userService = new UserClientService(networkDataHelper);

        var workProfileService = new WorkProfileClientService(networkDataHelper);

        //    new SendChatMessageOption("Enviar Mensaje", chatMessageService),
        //    new CheckChatMessagesOption("Revisar Mensajes", chatMessageService),
        //    new ShowUserOption("Buscar Perfil por ID", workProfileService),

        var enabledOptions = new List<IMenuOption>()
        {
            new CreateUserOption("Crear Usuario", userService),
            new CreateWorkProfileOption("Crear Perfil de Trabajo", workProfileService),
            new AssignImageToWorkProfile("Asignar Imagen a Perfil de Trabajo", workProfileService),
            new SearchWorkProfilesBySkillsOption("Buscar Perfiles por Habilidades", workProfileService),
            new SearchWorkProfilesByDescriptionOption("Buscar Perfiles por Descripción", workProfileService),
            new ExitOption("Salir", networkingManager)
        };

        IUIService uiService = new ConsoleMenuService(enabledOptions);

        uiService.Render();
    }
}
