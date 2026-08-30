namespace Travel.Web.DTOs.DashboardDtos
{
    public class PopularTourDto
    {
        public string TourId { get; set; }
        public string TourName { get; set; }
        public int ReservationCount { get; set; }
        public string CoverImage { get; set; }
    }
}
