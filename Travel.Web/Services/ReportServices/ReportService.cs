using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Net.NetworkInformation;
using Travel.Web.DTOs.ReportDtos;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Settings;

namespace Travel.Web.Services.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;

        public ReportService(IDatabaseSettings databaseSettings, IReservationService reservationService, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
            _reservationService = reservationService;
            _mapper = mapper;
        }

        public async Task<MemoryStream> ExportTourReservationsToExcelAsync(string tourId)
        {
            var dtos = await _reservationService.GetByTourIdAsync(tourId);
            return GenerateExcel(dtos);
        }

        public async Task<MemoryStream> ExportTourReservationsToPdfAsync(string tourId)
        {
            var dtos = await _reservationService.GetByTourIdAsync(tourId);
            return GeneratePdf(dtos);
        }

        public async Task<MemoryStream> ExportTourDateReservationsToExcelAsync(string tourId, string tourDateId, string status = null)
        {
            var dtos = await _reservationService.GetByTourIdAsync(tourId, tourDateId, status);
            return GenerateExcel(dtos);
        }

        public async Task<MemoryStream> ExportTourDateReservationsToPdfAsync(string tourId, string tourDateId, string status = null)
        {
            var dtos = await _reservationService.GetByTourIdAsync(tourId, tourDateId, status);
            return GeneratePdf(dtos);
        }

        private MemoryStream GenerateExcel(List<ResultReservationDto> dtos)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Tur Katılımcı Formu");

            string[] headers = { "Ad Soyad", "E-posta", "Telefon", "Tur", "Tur Tarihi",
                         "Yetişkin", "Çocuk", "Toplam Kişi", "Rezervasyon Tarihi",
                         "Toplam Ücret", "Durum" };

            for (int i = 0; i < headers.Length; i++)
                worksheet.Cell(1, i + 1).Value = headers[i];

            worksheet.Range("A1:K1").Style.Font.Bold = true;

            int row = 2;
            foreach (var item in dtos)
            {
                worksheet.Cell(row, 1).Value = item.UserFullName;
                worksheet.Cell(row, 2).Value = item.UserEmail;
                worksheet.Cell(row, 3).Value = item.UserPhone;
                worksheet.Cell(row, 4).Value = item.TourName;
                worksheet.Cell(row, 5).Value = item.TourDate.ToString("dd.MM.yyyy");
                worksheet.Cell(row, 6).Value = item.AdultCount;
                worksheet.Cell(row, 7).Value = item.ChildCount;
                worksheet.Cell(row, 8).Value = item.AdultCount + item.ChildCount;
                worksheet.Cell(row, 9).Value = item.CreatedAt.ToString("dd.MM.yyyy HH:mm");
                worksheet.Cell(row, 10).Value = item.TotalPrice;
                worksheet.Cell(row, 11).Value = item.Status.ToString();
                row++;
            }

            worksheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }

        private MemoryStream GeneratePdf(List<ResultReservationDto> dtos)
        {
            var stream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Text($"Tur Katılımcı Raporu PDF").FontSize(14).Bold().AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(5);
                            columns.RelativeColumn(5);
                            columns.RelativeColumn(6);
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(5);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(4);

                        });

                        table.Header(header =>
                        {
                            var headers = new[] { "Ad Soyad", "E-posta", "Telefon", "Tur", "Tur Tarihi",
                                         "Yetişkin", "Çocuk", "Toplam Kişi", "Rez. Tarihi",
                                         "Toplam Ücret", "Durum" };

                            foreach (var h in headers)
                            {
                                header.Cell().Background("#2563eb").Padding(5).Text(h).FontColor("#ffffff").Bold().FontSize(8);
                            }

                        });

                        bool isEven = false;
                        foreach (var item in dtos)
                        {
                            var bg = isEven ? "#f1f5f9" : "#ffffff";
                            isEven = !isEven;

                            var values = new[]
                            {
                                item.UserFullName,
                                item.UserEmail,
                                item.UserPhone,
                                item.TourName,
                                item.TourDate.ToString("dd.MM.yyyy"),
                                item.AdultCount.ToString(),
                                item.ChildCount.ToString(),
                                (item.AdultCount + item.ChildCount).ToString(),
                                item.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                                item.TotalPrice.ToString("N0") + "₺",
                                item.Status.ToString()
                            };

                            foreach (var val in values)
                            {
                                table.Cell().Background(bg).Padding(4).Text(val).FontSize(8);
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf(stream);

            stream.Position = 0;
            return stream;
        }

        
    }
}
