namespace LKDin.Exceptions
{
    public class WorkProfileAlreadyExistsException : Exception
    {
        public WorkProfileAlreadyExistsException(string userId) : base($"El usuario {userId} ya tiene un perfil de trabajo") { }
    }
}