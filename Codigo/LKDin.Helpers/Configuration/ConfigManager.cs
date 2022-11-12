using LKDin.Helpers.Assets;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace LKDin.Helpers.Configuration
{
    public static class ConfigManager
    {
        public static T GetConfig<T>(string configKey)
        {
            try
            {
                var configurationRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                return configurationRoot.GetValue<T>(configKey);
            }
            catch (ConfigurationErrorsException e)
            {
                throw new Exception($"Error al leer la configuración. Error: {e.Message}");
            }
        }

        public static string GetAppDataBasePath()
        {
            var folderPath = GetConfig<string>(ConfigConstants.APP_DATA_PATH_KEY);

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                folderPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "/LKDin");
            }

            AssetManager.EnsureFolderExists(folderPath);

            return folderPath;
        }

        public static string GetStoreFilePath(string storeName)
        {
            var storeFilePath = Path.Join(GetDataFolderPath(), storeName + ".data");

            return storeFilePath;
        }

        public static string GetDataFolderPath()
        {
            var path = GetAppDataBasePath();

            var storeFilePath = Path.Join(path, "/data");

            return storeFilePath;
        }

        public static string GetAssetsFolderPath(string storeName)
        {
            var path = GetAppDataBasePath();

            var assetsFolderPath = Path.Join(path + "/assets", storeName);

            return assetsFolderPath;
        }

        public static string GetTmpAssetsFolderPath()
        {
            var path = GetAppDataBasePath();

            var assetsFolderPath = Path.Join(path, "/tmp");

            return assetsFolderPath;
        }

        public static string GetDownloadsFolderPath(string storeName, bool isServer = false)
        {
            var path = "";

            if (isServer)
            {
                path = GetConfig<string>(ConfigConstants.DOWNLOADS_FOLDER_KEY) ?? "/LKDin-Server-Downloads";
            }
            else
            {
                path = GetConfig<string>(ConfigConstants.DOWNLOADS_FOLDER_KEY) ?? "/LKDin-Client-Downloads";
            }

            return Path.Join(path, storeName);
        }
    }
}
