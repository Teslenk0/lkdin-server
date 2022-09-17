using System.Xml.Linq;

namespace LKDin.Server.Domain
{
    public class ChatMessage : BaseEntity
    {
        public string Content { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public bool Read { get; set; }

        public override string Serialize()
        {
            return "Id=" + Id + "|" 
                   + "Content=" + Content + "|" 
                   + "SenderId=" + SenderId + "|" 
                   + "ReceiverId=" + ReceiverId + "|"
                   + "Read(boolean)=" + Read.ToString();
        }
    }
}