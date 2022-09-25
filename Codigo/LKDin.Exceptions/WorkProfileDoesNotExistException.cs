namespace LKDin.Exceptions
{
    public class WorkProfileDoesNotExistException : Exception
    {
        public WorkProfileDoesNotExistException(string message, bool isFullMessage = false) : base(isFullMessage ? message : $"El usuario {message} no tiene un perfil de trabajo") { }
    }
}