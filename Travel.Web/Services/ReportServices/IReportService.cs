namespace Travel.Web.Services.ReportServices
{
    public interface IReportService
    {
        Task<MemoryStream> ExportTourReservationsToExcelAsync(string tourId);
        Task<MemoryStream> ExportTourDateReservationsToExcelAsync(string tourId, string tourDateId, string status = null);

        Task<MemoryStream> ExportTourReservationsToPdfAsync(string tourId);
        Task<MemoryStream> ExportTourDateReservationsToPdfAsync(string tourId, string tourDateId, string status = null);
    }
}
