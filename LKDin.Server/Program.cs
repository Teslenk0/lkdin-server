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
            new CreateUserOption("Crear Usuario", new UserService()),
            new CreateWorkProfileOption("Crear Perfil de Trabajo", new WorkProfileService(new UserService())),
            new AssignImageToWorkProfile("Asignar Imagen a Perfil de Trabajo", new WorkProfileService(new UserService())),
            new SearchWorkProfilesBySkills("Buscar Perfiles por Habilidades", new WorkProfileService(new UserService())),
            new SearchWorkProfilesByDescription("Buscar Perfiles por Descripción", new WorkProfileService(new UserService())),
            new ExitOption("Salir")
        };

        IUIService uiService = new ConsoleMenuService(enabledOptions);

        Thread menuThread = new(uiService.Render);

        menuThread.Start();
    }
}

