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

            lock (locker)
            {
                var filePath = GetStoreFilePath(storeName);

                using FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None);

                using StreamWriter writer = new(file);

                writer.WriteLine(baseEntity.Serialize());

                writer.Flush();
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


            lock (locker)
            {
                var filePath = GetStoreFilePath(storeName);

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

        private static string GetStoreFilePath(string storeName)
        {
            var folder = Environment.SpecialFolder.ApplicationData;

            var path = Environment.GetFolderPath(folder);

            var skillsFilePath = Path.Join(path + "/LKDin", storeName + ".data");

            return skillsFilePath;
        }
    }
}
