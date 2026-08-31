using Travel.Web.DTOs.CategoryDtos;
using Travel.Web.DTOs.DestinationDtos;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Areas.Admin.Models
{
    public class TourListViewModel
    {
        public List<ResultTourDto> Tours { get; set; }
        public List<ResultDestinationDto> Destinations { get; set; }
        public List<ResultCategoryDto> Categories { get; set; }
    }
}
