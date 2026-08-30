using Travel.Web.Entities.Common;
using Travel.Web.Entities.Enums;

namespace Travel.Web.Entities
{
    public class Comment : BaseEntity
    {
        public string UserId { get; set; }
        public string TourId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public CommentStatus Status { get; set; } = CommentStatus.Bekliyor;
    }
}
