namespace Travel.Web.DTOs.FavoriteDtos
{
    public class CreateFavoriteDto
    {
        public string UserId { get; set; }
        public string TourId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
