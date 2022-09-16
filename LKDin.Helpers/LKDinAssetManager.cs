﻿using System.Diagnostics;
using System.IO;
using System.Text;
using LKDin.Exceptions;

namespace LKDin.Helpers
{
    public static class LKDinAssetManager
    {
        private readonly static Dictionary<string, object> _lockers = new();

        // Returns the new path for the file
        public static string CopyAssetToAssetsFolder<T>(string sourceFile, string id)
        {
            if(sourceFile == null)
            {
                throw new AssetDoesNotExistException(sourceFile);
            }

            var storeName = typeof(T).Name.ToLower();

            EnsureAssetFolderExistance(storeName);

            var destinationFile = LKDinConfigManager
        .GetAssetsFolderPath(storeName) + $"/{id}{Path.GetExtension(sourceFile)}";

            if (!_lockers.ContainsKey(sourceFile))
            {
                _lockers.TryAdd(sourceFile, new object());
            }

            if (!_lockers.ContainsKey(destinationFile))
            {
                _lockers.TryAdd(destinationFile, new object());
            }

            var lockerForSourceFile = _lockers[sourceFile];

            var lockerForDestinationFile = _lockers[destinationFile];

            lock (lockerForSourceFile)
            {
                lock (lockerForDestinationFile)
                {
                    if (!File.Exists(sourceFile))
                    {
                        throw new AssetDoesNotExistException(sourceFile);
                    }

                    File.Copy(sourceFile, destinationFile, true);
                }
            }

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

        private static void EnsureAssetFolderExistance(string storeName)
        {
            var folderDataPath = LKDinConfigManager.GetAssetsFolderPath(storeName);

            Directory.CreateDirectory(folderDataPath);
        }
    }
}
