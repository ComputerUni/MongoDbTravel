namespace Travel.Web.DTOs.DestinationDtos
{
    public class CreateDestinationDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsPopular { get; set; }
    }
}
