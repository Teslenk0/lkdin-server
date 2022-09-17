namespace LKDin.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base($"La contraseña es incorrecta") { }
    }
}