namespace Travel.Web.DTOs.CommonDtos
{
    public class TourDateDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quota { get; set; }
        public int RemainingQuota { get; set; }
        public bool IsActive { get; set; }
    }
}
