namespace LKDin.Server.Domain
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}