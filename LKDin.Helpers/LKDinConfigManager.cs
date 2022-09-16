using System.IO;
using System.Text;

namespace LKDin.Helpers
{
    public static class LKDinConfigManager
    {
        private static string GetBasePath()
        {
            var folder = Environment.SpecialFolder.ApplicationData;

            var path = Environment.GetFolderPath(folder);

            return path + "/LKDin";
        }

        public static string GetStoreFilePath(string storeName)
        {
            var storeFilePath = Path.Join(GetDataFolderPath(), storeName + ".data");

            return storeFilePath;
        }

        public static string GetDataFolderPath()
        {
            var path = GetBasePath();

            var storeFilePath = Path.Join(path, "/data");

            return storeFilePath;
        }

        public static string GetAssetsFolderPath(string storeName)
        {
            var path = GetBasePath();

            var assetsFolderPath = Path.Join(path + "/assets", storeName);

            return assetsFolderPath;
        }
    }
}
