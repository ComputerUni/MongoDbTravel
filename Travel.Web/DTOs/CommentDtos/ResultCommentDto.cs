namespace Travel.Web.DTOs.CommentDtos
{
    public class ResultCommentDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string TourId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }

        // Kullanıcı bilgileri
        public string UserFullName { get; set; }
        public string UserInitials { get; set; }

        // Tur bilgisi (profil sayfası için)
        public string TourName { get; set; }
    }
}
