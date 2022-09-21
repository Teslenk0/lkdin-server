using LKDin.DTOs;
using LKDin.Helpers.Utils;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace LKDin.Helpers.Serialization
{
    public class SerializationManager
    {
        private const string LIST_SEPARATOR = "::::";

        private const char FIELD_SEPARATOR = '|';

        private const char LIST_START_MARKER = '<';

        private const char LIST_END_MARKER = '>';

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
                    
                    if(listType == typeof(SkillDTO) && collection != null)
                    {
                        var serializedList = $"{field.Name}(list(skilldto))={LIST_START_MARKER}";

                        for (int j = 0; j < collection.Count; j++)
                        {
                            var serializedObj = Serialize<SkillDTO>(collection[j]);

                            serializedList += serializedObj;

                            if(j < collection.Count - 1)
                            {
                                serializedList += LIST_SEPARATOR;
                            }
                        }
                        serializedData += $"{serializedList}{LIST_END_MARKER}{FIELD_SEPARATOR}";
                    }
                }
                else
                {
                    serializedData += $"{field.Name}({propertyType.Name.ToLower()})={field.GetValue(entity)}";

                    if (i != fields.Length - 1)
                    {
                        serializedData += FIELD_SEPARATOR;
                    }
                }
            }

            return serializedData;
        }

        public static T Deserialize<T>(string rawSerializedEntity)
            where T : new()
        {
            var serializedEntity = rawSerializedEntity;

            var serializedListsQ = new Queue<string>();

            if (serializedEntity.Contains(LIST_END_MARKER) && serializedEntity.Contains(LIST_START_MARKER))
            {
                var amountOfLists = serializedEntity.Count(c => c == LIST_START_MARKER);

                for(int i = 0; i < amountOfLists; i++)
                {
                    var from = serializedEntity.IndexOf(LIST_START_MARKER);

                    var to = serializedEntity.IndexOf(LIST_END_MARKER);

                    // Get the string between LIST_START_MARKER and LIST_END_MARKER
                    var listContent = serializedEntity.Substring(from + 1, (to - from) - 1);

                    // Replace the string from LIST_START_MARKER to LIST_END_MARKER
                    serializedEntity = serializedEntity.ReplaceAt(from, (to - from) + 1, "");

                    // Add to FIFO queue
                    serializedListsQ.Enqueue(listContent);
                }
            }

            var fields = serializedEntity.Split(FIELD_SEPARATOR);

            var entity = new T();

            Type type = entity
                        .GetType();

            foreach (string field in fields)
            {
                var data = field.Split('=');

                // The field must explicitly say to which type should be deserialized.
                if (data[0].Contains("(boolean)"))
                {
                    var fieldName = data[0].Replace("(boolean)", "");

                    type.GetProperty(fieldName)
                        .SetValue(entity, bool.Parse(data[1]), null);
                }
                else if (data[0].Contains("(list(skilldto))"))
                {
                    var fieldName = data[0].Replace("(list(skilldto))", "");

                    var skills = new List<SkillDTO>();

                    // Get the first element from the Queue and removes it
                    var serializedList = serializedListsQ.Dequeue();

                    var serializedListObjects = serializedList.Split(LIST_SEPARATOR);

                    foreach(var item in serializedListObjects)
                    {
                        var skill = Deserialize<SkillDTO>(item);

                        skills.Add(skill);
                    }

                    type.GetProperty(fieldName)
                        .SetValue(entity, skills);
                }
                else if (data[0].Contains("(int64)"))
                {
                    var fieldName = data[0].Replace("(int64)", "");

                    type.GetProperty(fieldName)
                        .SetValue(entity, long.Parse(data[1] != "" ? data[1] : "0"), null);
                }
                else if (data[0].Contains("(string)"))
                {
                    var fieldName = data[0].Replace("(string)", "");

                    type.GetProperty(fieldName)
                        .SetValue(entity, data[1], null);
                }
            }

            return entity;
        }
    }
}
