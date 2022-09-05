namespace LKDin.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException(string userId) : base($"El usuario {userId} no existe") { }
    }
}