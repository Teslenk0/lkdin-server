namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class ExitOption : ConsoleMenuOption
    {
        public ExitOption(string messageToPrint) : base(messageToPrint)
        {
        }

        protected override void PerformExecution()
        {
            Console.WriteLine("Muchas gracias por utilizar LKDin!");

            // TODO: Check that there are no networking stuff going on;
            System.Environment.Exit(0);
        }
    }
}
