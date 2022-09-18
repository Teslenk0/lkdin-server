using System.Net.Sockets;

namespace LKDin.Helpers.Configuration
{
    public static class ConfigManager
    {
        private static readonly object _configLock = new();

        public static string? GetConfig(string configKey)
        {
            var basePath = GetAppDataBasePath();

            var configFilePath = Path.Join(basePath, "LKDin.config");

            lock (_configLock)
            {
                using FileStream file = new(configFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);

                using StreamReader reader = new(file);

                var found = false;

                string? configValue = null;

                while (!reader.EndOfStream && !found)
                {
                    var data = reader.ReadLine();

                    if (data != null && data != "")
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

        private static string GetAppDataBasePath()
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
    }
}
