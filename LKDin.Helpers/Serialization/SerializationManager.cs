using LKDin.DTOs;
using LKDin.Helpers.Utils;
using System.Collections;
using System.Reflection;

namespace LKDin.Helpers.Serialization
{
    public class SerializationManager
    {
        public static string Serialize<T>(object entity)
        {
            var serializedData = "";

            var fields = entity.GetType().GetProperties();

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];

                var propertyType = Nullable.GetUnderlyingType(field.PropertyType) ?? field.PropertyType;

                if (propertyType.IsGenericList())
                {
                    Type listType = propertyType.GetGenericArguments()[0];

                    var collection = (IList)field.GetValue(entity, null);
                    
                    if(listType == typeof(SkillDTO))
                    {
                        var serializedList = $"{field.Name}(list<skilldto>)=<";

                        for (int j = 0; j < collection.Count; j++)
                        {
                            var serializedObj = Serialize<SkillDTO>(collection[j]);

                            serializedList += serializedObj;

                            if(j < collection.Count - 1)
                            {
                                serializedList += ":::";
                            } else
                            {
                                serializedList += ">";
                            }
                        }
                        serializedData += serializedList;
                    }
                }
                else
                {
                    serializedData += $"{field.Name}({propertyType.Name.ToLower()})={field.GetValue(entity)}";

                    if (i != fields.Length - 1)
                    {
                        serializedData += "|";
                    }
                }
            }

            return serializedData;
        }

        public static T Deserialize<T>(string serializedEntity)
            where T : new()
        {
            var fields = serializedEntity.Split('|');

            var entity = new T();

            foreach (string field in fields)
            {
                var data = field.Split('=');

                // The field must explicitly say to which type should be deserialized.
                if (data[0].Contains("(boolean)"))
                {
                    var fieldName = data[0].Replace("(boolean)", "");
                    entity
                        .GetType()
                        .GetProperty(fieldName)
                        .SetValue(entity, bool.Parse(data[1]), null);
                }
                else if (data[0].Contains("(int64)"))
                {
                    var fieldName = data[0].Replace("(int64)", "");
                    entity
                        .GetType()
                        .GetProperty(fieldName)
                        .SetValue(entity, long.Parse(data[1] != "" ? data[1] : "0"), null);
                }
                else if (data[0].Contains("(string)"))
                {
                    var fieldName = data[0].Replace("(string)", "");

                    entity
                        .GetType()
                        .GetProperty(fieldName)
                        .SetValue(entity, data[1], null);
                }
            }

            return entity;
        }
    }
}
