using LKDin.Networking;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class ExitOption : ConsoleMenuOption
    {
        private readonly INetworkingManager _networkingManager;

        public ExitOption(string messageToPrint, INetworkingManager networkingManager) : base(messageToPrint)
        {
            this._networkingManager = networkingManager;
        }

        protected override async Task PerformExecution()
        {
            Console.WriteLine("Muchas gracias por utilizar LKDin!");

            this._networkingManager.ShutdownTCPConnections();

            Console.WriteLine("Saliendo...");

            Task.Delay(3000).Wait();

            // TODO: Check that there are no networking stuff going on;
            Environment.Exit(0);
        }
    }
}
