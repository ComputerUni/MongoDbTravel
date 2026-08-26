using MongoDB.Bson.Serialization.Attributes;

namespace Travel.Web.Entities
{
    public class TourDate
    {
        [BsonElement("Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quota { get; set; }
        public bool IsActive { get; set; }
    }
}
