namespace LKDin.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string message, bool isFullMessage = false) : base(isFullMessage ? message : $"El usuario {message} ya existe")
        { }
    }
}