using LKDin.Helpers.Assets;
using System.Net.Sockets;

namespace LKDin.Helpers.Configuration
{
    public static class ConfigManager
    {
        private static readonly object _configLock = new();

        private const string DOWNLOADS_FOLDER_KEY = "ABS_DOWNLOADS_PATH";

        public static string? GetConfig(string configKey, ConfigNameSpace configNameSpace)
        {
            var basePath = GetAppDataBasePath();

            string configFilePath;

            if(configNameSpace == ConfigNameSpace.SERVER)
            {
                configFilePath = Path.Join(basePath, "LKDin.server.config");
            } else
            {
                configFilePath = Path.Join(basePath, "LKDin.client.config");
            }

            lock (_configLock)
            {
                using FileStream file = new(configFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);

                using StreamReader reader = new(file);

                var found = false;

                string? configValue = null;

                while (!reader.EndOfStream && !found)
                {
                    var data = reader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        var config = data.Trim().Split("=");

                        if (config[0].ToUpper().Equals(configKey.ToUpper()))
                        {
                            found = true;

                            configValue = config[1].Trim();
                        }
                    }
                }

                return configValue;

            }
        }

        public static string GetAppDataBasePath()
        {
            var folder = Environment.SpecialFolder.ApplicationData;

            var path = Path.Join(Environment.GetFolderPath(folder), "/LKDin");

            AssetManager.EnsureFolderExists(path);

            return path;
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
                path = GetConfig(DOWNLOADS_FOLDER_KEY, ConfigNameSpace.SERVER) ?? "/LKDin-Server-Downloads";
            } else
            {
                path = GetConfig(DOWNLOADS_FOLDER_KEY, ConfigNameSpace.CLIENT) ?? "/LKDin-Client-Downloads";
            }

            return Path.Join(path, storeName);
        }
    }
}
