using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Question : BaseEntity
    {
        public string UserId { get; set; }
        public string TourId { get; set; }
        public string Content { get; set; }
        public string? Answer { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAnswered { get; set; }
    }
}
