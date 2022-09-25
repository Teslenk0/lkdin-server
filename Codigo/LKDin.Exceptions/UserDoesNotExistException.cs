namespace LKDin.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException(string message, bool isFullMessage = false) : base(isFullMessage ? message : $"El usuario {message} no existe") { }
    }
}