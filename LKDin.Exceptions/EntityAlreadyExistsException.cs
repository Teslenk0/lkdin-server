namespace LKDin.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string message, bool isFullMessage = false) : base(isFullMessage ? message : $"La entidad {message} ya existe")
        { }
    }
}