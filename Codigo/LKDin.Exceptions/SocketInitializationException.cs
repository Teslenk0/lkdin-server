namespace LKDin.Exceptions
{
    public class TCPConnectionInitializationException : Exception
    {
        public TCPConnectionInitializationException() : base("Tiene que inicializar la conexión antes de arrancar a aceptar conexiones") { }
    }
}