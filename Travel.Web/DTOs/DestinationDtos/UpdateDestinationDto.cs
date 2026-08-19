namespace Travel.Web.DTOs.DestinationDtos
{
    public class UpdateDestinationDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsPopular { get; set; }
    }
}
