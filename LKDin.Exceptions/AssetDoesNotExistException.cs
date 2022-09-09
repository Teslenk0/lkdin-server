namespace LKDin.Exceptions
{
    public class AssetDoesNotExistException : Exception
    {
        public AssetDoesNotExistException(string assetPath) : base($"El archivo {assetPath} no existe") { }
    }
}