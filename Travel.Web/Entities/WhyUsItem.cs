using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class WhyUsItem : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
    }
}
