namespace LKDin.Logging.Service.Internal.DTOs
{
    public class FilterParams
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public string? Level { get; set; }

        public string? NameSpace { get; set; }
    }
}
