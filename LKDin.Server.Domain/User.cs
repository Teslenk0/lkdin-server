namespace LKDin.Server.Domain
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public WorkProfile WorkProfile { get; set; }

        public ICollection<ChatMessage> ReceivedMessages { get; set; }

        public ICollection<ChatMessage> SentMessages { get; set; }
    }
}