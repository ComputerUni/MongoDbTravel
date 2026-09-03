namespace Travel.Web.DTOs.WhyUsItemDtos
{
    public class ResultWhyUsItemDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? TitleEn { get; set; }
        public string Description { get; set; }
        public string? DescriptionEn { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
    }
}
