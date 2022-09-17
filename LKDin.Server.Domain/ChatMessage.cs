namespace LKDin.Server.Domain
{
    public class ChatMessage : BaseEntity
    {
        public string Content { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public bool Read { get; set; }

        public long SentAt { get; set; }

        public long? ReadAt { get; set; }

        public override string Serialize()
        {
            var serializedChatMessage = "Id=" + Id + "|"
                   + "Content=" + Content + "|"
                   + "SenderId=" + SenderId + "|"
                   + "ReceiverId=" + ReceiverId + "|"
                   + "SentAt(long)=" + SentAt.ToString() + "|"
                   + "Read(boolean)=" + Read.ToString();

            if(ReadAt != null)
            {
                serializedChatMessage += "|ReadAt(long)=" + ReadAt.ToString();
            }

            return serializedChatMessage;
        }
    }
}