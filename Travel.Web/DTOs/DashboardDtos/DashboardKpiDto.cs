namespace Travel.Web.DTOs.DashboardDtos
{
    public class DashboardKpiDto
    {
        public int TotalTours { get; set; }
        public int ActiveTours { get; set; }
        public int PassiveTours { get; set; }
        public int TotalReservations { get; set; }
        public int ThisMonthReservations { get; set; }
        public int TotalUsers { get; set; }
        public int PendingQuestions { get; set; }
        public string MostReservedTourName { get; set; }
    }
}
