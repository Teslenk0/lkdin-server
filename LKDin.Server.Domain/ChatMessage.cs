namespace LKDin.Server.Domain
{
    public class ChatMessage : BaseEntity
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public User Sender { get; set; }

        public string SenderId { get; set; }

        public User Receiver { get; set; }

        public string ReceiverId { get; set; }

        public bool Read { get; set; }
    }
}