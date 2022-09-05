namespace LKDin.Exceptions
{
    public class WorkProfileDoesNotExistException : Exception
    {
        public WorkProfileDoesNotExistException(string userId) : base($"El usuario {userId} no tiene un perfil de trabajo") { }
    }
}