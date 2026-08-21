using Travel.Web.DTOs.CommonDtos;
using Travel.Web.Entities;

namespace Travel.Web.DTOs.TourDtos
{
    public class ResultTourDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Route { get; set; }
        public string TourType { get; set; }
        public int Night { get; set; }
        public int GroupSize { get; set; }
        public int MinParticipant { get; set; }
        public string DepartureCity { get; set; }
        public string Transport { get; set; }
        public string Accommodation { get; set; }
        public string GuideLanguage { get; set; }
        public string VisaInfo { get; set; }
        public decimal ChildPrice { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsBest { get; set; }
        public bool IsNew { get; set; }
        public List<string> Included { get; set; } = new();
        public List<string> NotIncluded { get; set; } = new();

        public decimal Price { get; set; }
        //public string Country { get; set; }
        //public string City { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string DestinationId { get; set; }
        public int Duration { get; set; }
        public string? CoverImage { get; set; }
        public string? IsActive { get; set; }
        public List<string?> Gallery { get; set; } = new();
        public List<string> Features { get; set; } = new();
        public List<TourDateDto> Dates { get; set; } = new();
        public List<DayProgramDto> DayPrograms { get; set; } = new();
    }
}
