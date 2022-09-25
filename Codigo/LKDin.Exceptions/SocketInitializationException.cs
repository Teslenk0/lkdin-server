namespace LKDin.Exceptions
{
    public class SocketInitializationException : Exception
    {
        public SocketInitializationException() : base("Tiene que inicializar el socket antes de arrancar a aceptar conexiones") { }
    }
}