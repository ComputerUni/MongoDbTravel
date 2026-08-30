using Travel.Web.DTOs.DestinationDtos;

namespace Travel.Web.Areas.User.Models
{
    public class HeroViewModel
    {
        public int DestinationCount { get; set; }
        public int TourCount { get; set; }
        public int TravelerCount { get; set; }
        public List<ResultDestinationDto> Destination { get; set; }
    }
}
