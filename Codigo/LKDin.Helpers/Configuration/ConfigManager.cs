using LKDin.Helpers.Assets;
using System.Configuration;

namespace LKDin.Helpers.Configuration
{
    public static class ConfigManager
    {
        public const string DOWNLOADS_FOLDER_KEY = "ABS_DOWNLOADS_PATH";

        public const string SERVER_PORT_KEY = "SERVER_PORT";

        public const string SERVER_IP_KEY = "SERVER_IP";

        public const string APP_DATA_PATH_KEY = "ABS_APP_DATA_PATH";

        public static string? GetConfig(string configKey)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[configKey] ?? string.Empty;
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine("Error al leer la configuración. Error: {0}", e.Message);
                return string.Empty;
            }
        }

        public static string GetAppDataBasePath()
        {
            var folderPath = GetConfig(APP_DATA_PATH_KEY);

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
                path = GetConfig(DOWNLOADS_FOLDER_KEY) ?? "/LKDin-Server-Downloads";
            } else
            {
                path = GetConfig(DOWNLOADS_FOLDER_KEY) ?? "/LKDin-Client-Downloads";
            }

            return Path.Join(path, storeName);
        }
    }
}
