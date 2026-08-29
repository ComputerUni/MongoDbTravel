namespace Travel.Web.DTOs.ReportDtos
{
    public class TourParticipantReportDto
    {
        public string UserFullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TourName { get; set; }
        public string TourDateText { get; set; } 
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int TotalPerson { get; set; }
        public DateTime ReservationDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
