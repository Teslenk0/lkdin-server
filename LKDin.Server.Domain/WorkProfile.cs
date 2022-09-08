using System.Xml.Linq;

namespace LKDin.Server.Domain
{
    public class WorkProfile : BaseEntity
    {
        public Guid Id { get; set; }

        public ICollection<Skill> Skills { get; set; }

        public string Description { get; set; }

        public string? ImagePath { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public override string Serialize()
        {
            return "Id=" + Id.ToString() + "|"
                   + "Description=" + Description + "|"
                   + "ImagePath=" + ImagePath + "|"
                   + "UserId=" + UserId;
        }
    }
}