using Travel.Web.DTOs.CommonDtos;

namespace Travel.Web.DTOs.TourDtos
{
    public class UpdateTourDto
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
        public string MeetingPoint { get; set; }
        public decimal ChildPrice { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsBest { get; set; }
        public bool IsNew { get; set; }
        public List<string> Included { get; set; } = new();
        public List<string> NotIncluded { get; set; } = new();
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public string DestinationId { get; set; }
        public int Duration { get; set; }
        public IFormFile? CoverImage { get; set; }
        public string? ExistingCoverImage { get; set; }
        public string? IsActive { get; set; }
        public List<IFormFile>? Gallery { get; set; } = new();
        public List<string> ExistingGallery { get; set; } = new();
        public List<string> Features { get; set; } = new();
        public List<TourDateDto> Dates { get; set; } = new();
        public List<DayProgramDto> DayPrograms { get; set; } = new();


        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
        public string? ShortDescriptionEn { get; set; }
        public string? RouteEn { get; set; }
        public string? TourTypeEn { get; set; }
        public string? TransportEn { get; set; }
        public string? AccommodationEn { get; set; }
        public string? GuideLanguageEn { get; set; }
        public string? VisaInfoEn { get; set; }
        public string? MeetingPointEn { get; set; }
        public List<string>? IncludedEn { get; set; } = new();
        public List<string>? NotIncludedEn { get; set; } = new();
        public List<DayProgramDto>? DayProgramsEn { get; set; } = new();
        public List<string>? FeaturesEn { get; set; } = new();
    }
}
