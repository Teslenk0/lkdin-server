namespace LKDin.Server.Domain
{
    public class User : BaseEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public override string Serialize()
        {
            return "Id=" + Id + "|"
                   + "Name=" + Name + "|"
                   + "Password=" + Password;
        }
    }
}