namespace LKDin.Server.Domain
{
    public class Skill : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string WorkProfileId { get; set; }

        public override string Serialize()
        {
            return "Id=" + Id.ToString() + "|"
                    + "Name=" + Name + "|"
                    + "WorkProfileId=" + WorkProfileId + "|";
        }
    }
}