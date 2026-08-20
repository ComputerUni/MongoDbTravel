namespace Travel.Web.Entities
{
    public class TourDate
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quota { get; set; }
        public int RemainingQuota { get; set; }
        public bool IsActive { get; set; }
    }
}
