namespace LKDin.Exceptions
{
    public class NoImageAssignedException : Exception
    {
        public NoImageAssignedException(string message, bool isFullMessage = false) : base(isFullMessage ? message : $"El perfil del usuario {message} no tiene una imagen asignada")
        { }
    }
}