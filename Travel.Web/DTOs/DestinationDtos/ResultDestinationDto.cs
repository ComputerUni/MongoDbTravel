namespace Travel.Web.DTOs.DestinationDtos
{
    public class ResultDestinationDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsPopular { get; set; }

        public string? NameEn { get; set; }
        public string? CountryEn { get; set; }
        public string? DescriptionEn { get; set; }
    }
}
