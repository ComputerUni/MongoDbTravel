using Travel.Web.DTOs.CategoryDtos;
using Travel.Web.DTOs.DestinationDtos;
using Travel.Web.DTOs.LookupDtos;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Areas.Admin.Models
{
    public class UpdateTourViewModel
    {
        public UpdateTourDto Tour { get; set; } = new();
        public List<ResultCategoryDto> Categories { get; set; } = new();
        public List<ResultDestinationDto> Destinations { get; set; } = new();

        public List<ResultLookupDto> TourTypes { get; set; } = new();
        public List<ResultLookupDto> Cities { get; set; } = new();
        public List<ResultLookupDto> Transports { get; set; } = new();
        public List<ResultLookupDto> GuideLanguages { get; set; } = new();
        public List<ResultLookupDto> VisaInfos { get; set; } = new();
    }
}
