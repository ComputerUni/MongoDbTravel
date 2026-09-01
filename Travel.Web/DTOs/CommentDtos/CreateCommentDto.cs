namespace Travel.Web.DTOs.CommentDtos
{
    public class CreateCommentDto
    {
        public string? UserId { get; set; }
        public string TourId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
    }
}
