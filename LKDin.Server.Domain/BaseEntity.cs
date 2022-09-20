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

                // The field may explicitly say to which type should be deserialized.
                if (data[0].Contains("(boolean)"))
                {
                    var fieldName = data[0].Replace("(boolean)", "");
                    entity
                        .GetType()
                        .GetProperty(fieldName)
                        .SetValue(entity, Boolean.Parse(data[1]), null);
                }
                else if (data[0].Contains("(long)"))
                {
                    var fieldName = data[0].Replace("(long)", "");
                    entity
                        .GetType()
                        .GetProperty(fieldName)
                        .SetValue(entity, Int64.Parse(data[1]), null);
                }
                else
                {
                    entity
                        .GetType()
                        .GetProperty(data[0])
                        .SetValue(entity, data[1], null);
                }
            }

            return entity;
        }
    }
}
