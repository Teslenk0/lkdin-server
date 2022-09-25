using LKDin.Exceptions;
using LKDin.Helpers.Configuration;

namespace LKDin.Helpers.Assets
{
    public static class AssetManager
    {
        private readonly static Dictionary<string, object> _lockers = new();

        public static string CopyFileToDownloadsFolder<T>(string filePath, bool isServer)
        {
            if (!DoesFileExist(filePath))
            {
                throw new AssetDoesNotExistException(filePath);
            }

            var storeName = typeof(T).Name.ToLower();

            EnsureDownloadFolderExistance(storeName, isServer);

            var destinationPath = Path.Join(ConfigManager.GetDownloadsFolderPath(storeName, isServer), GetFileName(filePath));

            CopyAsset(filePath, destinationPath);

            return destinationPath;
        }

        // Returns the new path for the file
        public static string CopyAssetToAssetsFolder<T>(string sourceFile, string resultFileName)
        {
            if (!DoesFileExist(sourceFile))
            {
                throw new AssetDoesNotExistException(sourceFile);
            }

            var storeName = typeof(T).Name.ToLower();

            EnsureAssetFolderExistance(storeName);

            var destinationFile = ConfigManager.GetAssetsFolderPath(storeName) + $"/{resultFileName}{Path.GetExtension(sourceFile)}";

            CopyAsset(sourceFile, destinationFile);

            return destinationFile;
        }

        public static bool DoesFileExist(string filePath)
        {
            if (!_lockers.ContainsKey(filePath))
            {
                _lockers.TryAdd(filePath, new object());
            }

            lock (filePath)
            {
                return File.Exists(filePath);
            }
        }

        public static byte[] ReadAsset(string filePath, long offset, int length)
        {
            if (DoesFileExist(filePath))
            {
                if (!_lockers.ContainsKey(filePath))
                {
                    _lockers.TryAdd(filePath, new object());
                }

                lock (filePath)
                {
                    var data = new byte[length];

                    using var fs = new FileStream(filePath, FileMode.Open) { Position = offset };

                    var bytesRead = 0;

                    while (bytesRead < length)
                    {
                        var read = fs.Read(data, bytesRead, length - bytesRead);
                        if (read == 0)
                            throw new Exception("Error al leer el archivo");
                        bytesRead += read;
                    }

                    return data;
                }
            }

            throw new AssetDoesNotExistException(filePath);
        }

        public static string WriteAssetToTmp(string fileName, byte[] data)
        {
            EnsureTmpAssetFolderExistance();

            var filePath = Path.Join(ConfigManager.GetTmpAssetsFolderPath(), fileName);

            var fileMode = DoesFileExist(filePath) ? FileMode.Append : FileMode.Create;

            var finalFileName = fileName;

            if(fileMode == FileMode.Create)
            {
                // If the file does not exists in the tmp folder assign a tmp name
                finalFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

                filePath = filePath.Replace(fileName, finalFileName);
            }

            if (!_lockers.ContainsKey(filePath))
            {
                _lockers.TryAdd(filePath, new object());
            }

            lock (_lockers[filePath])
            {
                using var fs = new FileStream(filePath, fileMode);

                fs.Write(data, 0, data.Length);
            }          
            
            return finalFileName;
        }

        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public static long GetFileSize(string filePath)
        {
            if (DoesFileExist(filePath))
            {
                return new FileInfo(filePath).Length;
            }

            throw new AssetDoesNotExistException(filePath);
        }

        private static void CopyAsset(string sourcePath, string destinationPath)
        {
            if (!_lockers.ContainsKey(sourcePath))
            {
                _lockers.TryAdd(sourcePath, new object());
            }

            if (!_lockers.ContainsKey(destinationPath))
            {
                _lockers.TryAdd(destinationPath, new object());
            }

            var lockerForSourceFile = _lockers[sourcePath];

            var lockerForDestinationFile = _lockers[destinationPath];

            lock (lockerForSourceFile)
            {
                lock (lockerForDestinationFile)
                {
                    File.Copy(sourcePath, destinationPath, true);
                }
            }
        }

        private static void EnsureAssetFolderExistance(string storeName)
        {
            var folderDataPath = ConfigManager.GetAssetsFolderPath(storeName);

            EnsureFolderExists(folderDataPath);
        }

        private static void EnsureDownloadFolderExistance(string storeName, bool isServer)
        {
            var folderDataPath = ConfigManager.GetDownloadsFolderPath(storeName, isServer);

            EnsureFolderExists(folderDataPath);
        }

        private static void EnsureTmpAssetFolderExistance()
        {
            var folderDataPath = ConfigManager.GetTmpAssetsFolderPath();

            EnsureFolderExists(folderDataPath);
        }

        public static void EnsureFolderExists(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
        }
    }
}
