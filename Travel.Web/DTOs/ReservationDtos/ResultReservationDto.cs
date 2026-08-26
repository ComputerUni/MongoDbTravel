using Travel.Web.Entities.Enums;

namespace Travel.Web.DTOs.ReservationDtos
{
    public class ResultReservationDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string TourId { get; set; }
        public string TourDateId { get; set; }
        public DateTime TourDate { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TourName { get; set; }
        public string CoverImage { get; set; }
        public int Duration { get; set; }
        public string DestinationName { get; set; }
        public string TourIsActive { get; set; }
    }
}
