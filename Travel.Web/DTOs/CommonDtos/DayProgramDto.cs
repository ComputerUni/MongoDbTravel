namespace Travel.Web.DTOs.CommonDtos
{
    public class DayProgramDto
    {
        public int DayNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string? Transport { get; set; }
        public string? Meals { get; set; }
    }
}
