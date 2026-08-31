namespace Travel.Web.DTOs.FavoriteDtos
{
    public class ResultFavoriteDto
    {
        public string UserId { get; set; }
        public string TourId { get; set; }
        public DateTime CreatedAt { get; set; }

        //Tur bilgileri için
        public string TourName { get; set; }
        public string CoverImage { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Country { get; set; }
        public double AverageRating { get; set; }
    }
}
