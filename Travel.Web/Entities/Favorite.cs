using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Favorite : BaseEntity
    {
        public string UserId { get; set; }
        public string TourId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
