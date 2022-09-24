using LKDin.Exceptions;
using LKDin.Helpers.Configuration;
using LKDin.Helpers.Serialization;
using LKDin.Server.Domain;
using System.IO;
using System.Text;

namespace LKDin.Server.DataAccess
{
    internal static class DataManager
    {
        private readonly static Dictionary<string, object> _lockers = new();

        public static List<User> Users { get { return ReadDataFromStore<User>("user"); } }

        public static List<WorkProfile> WorkProfiles { get { return ReadDataFromStore<WorkProfile>("workprofile"); } }

        public static List<Skill> Skills { get { return ReadDataFromStore<Skill>("skill"); } }

        public static List<ChatMessage> ChatMessages { get { return ReadDataFromStore<ChatMessage>("chatmessage"); } }

        private static void EnsureStorageExistance(string storeName)
        {
            var folderDataPath = ConfigManager.GetDataFolderPath();

            Directory.CreateDirectory(folderDataPath);

            var storePath = ConfigManager.GetStoreFilePath(storeName);

            if (!File.Exists(storePath))
            {
                using FileStream fileStream = File.Create(storePath);

                fileStream.Flush();
            }
        }

        public static void AddDataToStore<T>(BaseEntity baseEntity)
        {
            var storeName = typeof(T).Name.ToLower();

            EnsureStorageExistance(storeName);

            if (!_lockers.ContainsKey(storeName))
            {
                _lockers.TryAdd(storeName, new object());
            }

            var filePath = ConfigManager.GetStoreFilePath(storeName);

            var locker = _lockers[storeName];

            lock (locker)
            {
                var existenceDoubleCheck = File.ReadLines(filePath)
                     .Any(l => l.Contains($"Id(string)={baseEntity.Id}"));

                var type = typeof(T);

                var secondType = typeof(User);

                if (existenceDoubleCheck)
                {
                    if(typeof(T) == typeof(User))
                    {
                        throw new UserAlreadyExistsException(baseEntity.Id);
                    } else if (typeof(T) == typeof(WorkProfile))
                    {
                        throw new WorkProfileAlreadyExistsException(baseEntity.Id);
                    } else
                    {
                        throw new EntityAlreadyExistsException(baseEntity.Id);
                    }
                }

                using FileStream file = new(filePath, FileMode.Append, FileAccess.Write, FileShare.None);

                using StreamWriter writer = new(file);

                writer.WriteLine(SerializationManager.Serialize<T>(baseEntity));

                writer.Flush();
            }
        }

        public static void UpdateDataFromStore<T>(BaseEntity baseEntity)
        {
            var storeName = typeof(T).Name.ToLower();

            if (!_lockers.ContainsKey(storeName))
            {
                _lockers.TryAdd(storeName, new object());
            }

            var filePath = ConfigManager.GetStoreFilePath(storeName);

            var locker = _lockers[storeName];

            lock (locker)
            {
                var linesToKeep = File.ReadLines(filePath)
                         .Where(l => !l.Contains($"Id(string)={baseEntity.Id}"))
                         .ToList();

                var updatedData = SerializationManager.Serialize<T>(baseEntity);

                linesToKeep.Add(updatedData);

                File.WriteAllLines(filePath, linesToKeep);
            }
        }

        private static List<T> ReadDataFromStore<T>(string storeName) where T : new()
        {
            EnsureStorageExistance(storeName);

            if (!_lockers.ContainsKey(storeName))
            {
                _lockers.TryAdd(storeName, new object());
            }

            var locker = _lockers[storeName];

            List<T> parsedDataList = new();

            var filePath = ConfigManager.GetStoreFilePath(storeName);

            lock (locker)
            {
                using FileStream file = new(filePath, FileMode.Open, FileAccess.Read, FileShare.None);

                using StreamReader reader = new(file);

                while (!reader.EndOfStream)
                {
                    var data = reader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        parsedDataList.Add(SerializationManager.Deserialize<T>(data));
                    }
                }

            }

            return parsedDataList;
        }
    }
}
