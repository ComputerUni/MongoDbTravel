namespace Travel.Web.DTOs.ReservationDtos
{
    public class ReservationKpiDto
    {
        public int TotalReservationCount { get; set; }
        public int ApprovedReservationCount { get; set; }
        public int PendingReservationCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
