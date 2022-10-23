using LKDin.IUI;

namespace LKDin.UI.ConsoleMenu;

public class ConsoleMenuService : IUIService
{
    private readonly List<IMenuOption> MenuOptions;

    private readonly bool IsServer;

    public ConsoleMenuService(List<IMenuOption> menuOptions, bool isServer)
    {
        MenuOptions = menuOptions;

        IsServer = isServer;
    }

    public async Task Render()
    {
        while (true)
        {
            Console.WriteLine("----------------------------");

            if (IsServer)
            {
                Console.WriteLine("    LKDin Server (v1.0.0)   ");

            } else
            {
                Console.WriteLine("    LKDin Client (v1.0.0)   ");
            }

            Console.WriteLine("----------------------------");

            for (var i = 0; i < MenuOptions.Count; i++)
            {

                MenuOptions[i].OptionNumber = i + 1;

                MenuOptions[i].PrintMenuOptionMessage();
            }

            Console.WriteLine("----------------------------");

            Console.Write("Ingresa la opcion deseada: ");

            var selectedOption = Console.ReadLine();

            Console.Clear();

            try
            {
                var parsedOption = Int32.Parse(selectedOption);

                if(parsedOption > MenuOptions.Count)
                {
                    throw new Exception("Unsupported option: {0}");
                }

                var optionHandler = MenuOptions[parsedOption - 1];

                await optionHandler.Execute();

                Console.Clear();
            }
            catch (Exception)
            {
                Console.WriteLine("==========================");
                Console.WriteLine("ERROR: Opcion no soportada");
                Console.WriteLine("==========================");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}