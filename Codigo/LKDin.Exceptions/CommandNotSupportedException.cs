namespace LKDin.Exceptions
{
    public class CommandNotSupportedException : Exception
    {
        public CommandNotSupportedException(string message, bool isFullMessage = false) : base(isFullMessage ? message : $"Comando {message} no soportado") { }
    }
}