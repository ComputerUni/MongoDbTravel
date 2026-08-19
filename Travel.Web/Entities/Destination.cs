using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Destination : BaseEntity
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsPopular { get; set; }
    }
}
