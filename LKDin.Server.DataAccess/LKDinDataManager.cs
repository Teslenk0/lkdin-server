using LKDin.Server.Domain;
using System.IO;
using System.Text;

namespace LKDin.Server.DataAccess
{
    public static class LKDinDataManager
    {
        private readonly static Dictionary<string, object> _lockers = new();

        public static List<User> Users { get { return ReadDataFromStore<User>(); } }

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

                using (FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
                {

                    StreamWriter writer = new(file);

                    writer.WriteLine(baseEntity.Serialize());

                    writer.Flush();

                    file.Close();
                }
            }
        }

        private static List<T> ReadDataFromStore<T>()
        {
            var storeName = typeof(T).Name.ToLower();

            if (!_lockers.ContainsKey(storeName))
            {
                _lockers.TryAdd(storeName, new object());
            }

            var locker = _lockers[storeName];

            List<T> baseEntities = new();

            lock (locker)
            {
                var filePath = GetStoreFilePath(storeName);

                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    StreamReader reader = new(file);

                    reader.ReadLine();
                    
                    file.Close();
                }
            }

            return baseEntities;
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
