using LKDin.DTOs;
using LKDin.Helpers.Utils;
using System.Collections;

namespace LKDin.Helpers.Serialization
{
    public class SerializationManager
    {
        private const string LIST_SEPARATOR = "::::";

        private const string NESTED_LIST_SEPARATOR = ":_:";

        private const char FIELD_SEPARATOR = '|';

        private const char LIST_START_MARKER = '<';

        private const char LIST_END_MARKER = '>';

        public static string Serialize<T>(object entity, string separator = LIST_SEPARATOR)
        {
            var serializedData = "";

            if (entity.IsGenericList())
            {
                var list = (IList)entity;

                serializedData += $"{LIST_START_MARKER}";

                for (int j = 0; j < list.Count; j++)
                {
                    var serializedObj = Serialize<T>(list[j]);

                    serializedData += serializedObj;

                    if (j < list.Count - 1)
                    {
                        serializedData += separator;
                    }
                }

                serializedData += $"{LIST_END_MARKER}";
            }
            else
            {
                var fields = entity.GetType().GetProperties();

                for (int i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];

                    var propertyType = Nullable.GetUnderlyingType(field.PropertyType) ?? field.PropertyType;

                    if (propertyType.IsGenericList())
                    {
                        Type listType = propertyType.GetGenericArguments()[0];

                        var collection = (IList)field.GetValue(entity, null);

                        if (listType == typeof(SkillDTO) && collection != null)
                        {
                            serializedData += $"{field.Name}(list(skilldto))={Serialize<List<SkillDTO>>(collection, NESTED_LIST_SEPARATOR)}";
                        }
                    }
                    else
                    {
                        serializedData += $"{field.Name}({propertyType.Name.ToLower()})={field.GetValue(entity)}";
                    }

                    if (i != fields.Length - 1)
                    {
                        serializedData += FIELD_SEPARATOR;
                    }
                }
            }

            return serializedData;
        }

        public static T DeserializeList<T>(string rawSerializedList, string separator = LIST_SEPARATOR) where T : new()
        {
            var genericList = (IList)new T();

            Type type = genericList.GetType();

            Type listType = type.GetGenericArguments()[0];

            // Remove fist and last markers (can't use replace)
            var serializedListObjects = rawSerializedList.Substring(1, rawSerializedList.Length - 2).Split(separator);

            foreach (var item in serializedListObjects)
            {
                if (listType == typeof(SkillDTO))
                {
                    genericList.Add(Deserialize<SkillDTO>(item));
                } else if (listType == typeof(WorkProfileDTO)) 
                {
                    genericList.Add(Deserialize<WorkProfileDTO>(item));
                } else if(listType == typeof(ChatMessageDTO))
                {
                    genericList.Add(Deserialize<ChatMessageDTO>(item));
                }
            }

            return (T)genericList;
        }

        public static T Deserialize<T>(string rawSerializedEntity)
            where T : new()
        {
            var entity = new T();

            if (entity.IsGenericList())
            {
                return DeserializeList<T>(rawSerializedEntity);
            }

            Type type = entity.GetType();

            var serializedEntity = rawSerializedEntity;

            var serializedListsQ = new Queue<string>();

            // If the serialized entity contains a list
            if (serializedEntity.Contains(LIST_END_MARKER) && serializedEntity.Contains(LIST_START_MARKER))
            {
                // Count the amount of lists available
                var amountOfLists = serializedEntity.Count(c => c == LIST_START_MARKER);

                for (int i = 0; i < amountOfLists; i++)
                {
                    // Take the start position of the first list 
                    var from = serializedEntity.IndexOf(LIST_START_MARKER);

                    // Take the end position of the first list
                    var to = serializedEntity.IndexOf(LIST_END_MARKER);

                    // Get the string between LIST_START_MARKER and LIST_END_MARKER
                    var listContent = serializedEntity[from..to];

                    // Remove the string from LIST_START_MARKER to LIST_END_MARKER so it's not deserialized more than one time
                    serializedEntity = serializedEntity.ReplaceAt(from, (to - from) + 1, "");

                    // Add to FIFO queue so it can be processed
                    serializedListsQ.Enqueue(listContent);
                }
            }

            var fields = serializedEntity.Split(FIELD_SEPARATOR);

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

                    // Get the first element from the Queue and removes it
                    var serializedList = serializedListsQ.Dequeue();

                    var deserializedList = DeserializeList<List<SkillDTO>>(serializedList, NESTED_LIST_SEPARATOR);

                    type.GetProperty(fieldName)
                        .SetValue(entity, deserializedList);
                }
                else if (data[0].Contains("(list(chatmessagedto))"))
                {
                    var fieldName = data[0].Replace("(list(chatmessagedto))", "");

                    // Get the first element from the Queue and removes it
                    var serializedList = serializedListsQ.Dequeue();

                    var deserializedList = DeserializeList<List<ChatMessageDTO>>(serializedList);

                    type.GetProperty(fieldName)
                        .SetValue(entity, deserializedList);
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
