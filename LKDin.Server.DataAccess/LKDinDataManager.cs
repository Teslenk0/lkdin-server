using LKDin.Helpers;
using LKDin.Server.Domain;
using System.IO;
using System.Text;

namespace LKDin.Server.DataAccess
{
    public static class LKDinDataManager
    {
        private readonly static Dictionary<string, object> _lockers = new();

        public static List<User> Users { get { return ReadDataFromStore<User>("user"); } }

        public static List<WorkProfile> WorkProfiles { get { return ReadDataFromStore<WorkProfile>("workprofile"); } }

        public static List<Skill> Skills { get { return ReadDataFromStore<Skill>("skill"); } }

        public static List<ChatMessage> ChatMessages { get { return ReadDataFromStore<ChatMessage>("chatmessage"); } }

        public static void AddDataToStore<T>(BaseEntity baseEntity)
        {
            var storeName = typeof(T).Name.ToLower();

            if (!_lockers.ContainsKey(storeName))
            {
                _lockers.TryAdd(storeName, new object());
            }

            var locker = _lockers[storeName];

            var filePath = LKDinConfigManager.GetStoreFilePath(storeName);

            lock (locker)
            {
                using FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None);

                using StreamWriter writer = new(file);

                writer.WriteLine(baseEntity.Serialize());

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

            var filePath = LKDinConfigManager.GetStoreFilePath(storeName);

            var locker = _lockers[storeName];

            lock (locker)
            {
                var linesToKeep = File.ReadLines(filePath)
                         .Where(l => !l.Contains($"Id={baseEntity.Id}"))
                         .ToList();

                var updatedData = baseEntity.Serialize();

                linesToKeep.Add(updatedData);

                File.WriteAllLines(filePath, linesToKeep);
            }
        }

        private static List<T> ReadDataFromStore<T>(string storeName) where T : new()
        {
            if (!_lockers.ContainsKey(storeName))
            {
                _lockers.TryAdd(storeName, new object());
            }

            var locker = _lockers[storeName];

            List<T> parsedDataList = new();

            var filePath = LKDinConfigManager.GetStoreFilePath(storeName);

            lock (locker)
            {
                using FileStream file = new(filePath, FileMode.Open, FileAccess.Read, FileShare.None);

                using StreamReader reader = new(file);

                while (!reader.EndOfStream)
                {
                    var data = reader.ReadLine();

                    if (data != null && data != "")
                    {
                        parsedDataList.Add(BaseEntity.Deserialize<T>(data));
                    }
                }

            }

            return parsedDataList;
        }
    }
}
