namespace LKDin.Server.Domain
{
    public class WorkProfile : BaseEntity
    {
        public ICollection<Skill> Skills { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }
    }
}