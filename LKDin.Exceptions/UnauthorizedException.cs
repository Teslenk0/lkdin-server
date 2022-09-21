namespace LKDin.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("La contrasena es incorrecta") { }
    }
}