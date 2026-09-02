using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class TourLocalization : BaseEntity
    {
        public string TourId { get; set; }   
        public string LanguageCode { get; set; } 

        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Route { get; set; }
        public string TourType { get; set; }
        public string Transport { get; set; }
        public string Accommodation { get; set; }
        public string GuideLanguage { get; set; }
        public string VisaInfo { get; set; }
        public string MeetingPoint { get; set; }

        public List<string> Included { get; set; } = new();
        public List<string> NotIncluded { get; set; } = new();
        public List<string> Features { get; set; } = new();
        public List<LocalizedDayProgram> DayPrograms { get; set; } = new();
    }

}
