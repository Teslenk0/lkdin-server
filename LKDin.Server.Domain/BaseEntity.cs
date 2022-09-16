using System.Text;

namespace LKDin.Server.Domain
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }

        public abstract string Serialize();

        protected byte[] SerializeAndEncode()
        {
            string serializedObject = Serialize();

            return new UTF8Encoding(true).GetBytes(serializedObject);
        }

        public static T Deserialize<T>(string serializedEntity)
            where T : new()
        {
            var fields = serializedEntity.Split('|');

            var entity = new T();

            foreach (string field in fields)
            {
                var data = field.Split('=');

                entity.GetType()
                    .GetProperty(data[0])
                    .SetValue(entity, data[1], null);
                
            }

            return entity;
        }
    }
}
