using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class LookupItem : BaseEntity
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool IsActive { get; set; }
    }
}
