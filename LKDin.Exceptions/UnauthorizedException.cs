namespace LKDin.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base($"La contaseña es incorrecta") { }
    }
}