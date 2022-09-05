namespace LKDin.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string userId) : base($"El usuario {userId} ya existe") { }
    }
}