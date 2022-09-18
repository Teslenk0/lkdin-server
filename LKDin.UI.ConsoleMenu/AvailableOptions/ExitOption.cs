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

        protected override void PerformExecution()
        {
            Console.WriteLine("Muchas gracias por utilizar LKDin!");

            this._networkingManager.ShutdownSocketConnections();

            // TODO: Check that there are no networking stuff going on;
            System.Environment.Exit(0);
        }
    }
}
