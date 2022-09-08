using System;
namespace LKDin.Helpers
{
    public static class LKDinSerializer
    {
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

