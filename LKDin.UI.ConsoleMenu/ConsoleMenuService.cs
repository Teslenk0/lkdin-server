using LKDin.IUI;

namespace LKDin.UI.ConsoleMenu;

public class ConsoleMenuService : IUIService
{
    private readonly List<IMenuOption> MenuOptions;

    public ConsoleMenuService(List<IMenuOption> menuOptions)
    {
        MenuOptions = menuOptions;
    }

    public void Render()
    {
        while (true)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("    LKDin (v1.0.0)   ");
            Console.WriteLine("----------------------");

            for (var i = 0; i < MenuOptions.Count; i++)
            {

                MenuOptions[i].OptionNumber = i + 1;

                MenuOptions[i].PrintMenuOptionMessage();
            }

            Console.WriteLine("---------------------");

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

                optionHandler.Execute();

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