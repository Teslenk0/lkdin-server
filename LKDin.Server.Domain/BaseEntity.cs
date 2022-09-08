using System.Text;

namespace LKDin.Server.Domain
{
    public abstract class BaseEntity
    {
        public abstract string Serialize();

        protected byte[] SerializeAndEncode()
        {
            string serializedObject = Serialize();

            return new UTF8Encoding(true).GetBytes(serializedObject);
        }
    }
}
