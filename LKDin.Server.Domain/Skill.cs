namespace LKDin.Server.Domain
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; }

        public string WorkProfileId { get; set; }

        public override string Serialize()
        {
            return "Id=" + Id + "|"
                    + "Name=" + Name + "|"
                    + "WorkProfileId=" + WorkProfileId;
        }
    }
}