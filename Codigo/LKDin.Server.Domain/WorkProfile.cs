using System.Xml.Linq;

namespace LKDin.Server.Domain
{
    public class WorkProfile : BaseEntity
    {
        public string Description { get; set; }

        public string? ImagePath { get; set; }

        public string UserId { get; set; }
    }
}