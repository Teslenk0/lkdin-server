namespace LKDin.Exceptions
{
    public class AssetDoesNotExistException : Exception
    {
        public AssetDoesNotExistException(string message, bool isFullMessage = false) : base(isFullMessage ? message : $"El archivo {message} no existe") { }
    }
}