namespace LKDin.Exceptions
{
    public class WorkProfileAlreadyExistsException : Exception
    {
        public WorkProfileAlreadyExistsException(string message, bool isFullMessage = false) : base(isFullMessage ? message : $"El usuario {message} ya tiene un perfil de trabajo") { }
    }
}