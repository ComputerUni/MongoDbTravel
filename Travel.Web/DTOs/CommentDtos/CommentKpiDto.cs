namespace Travel.Web.DTOs.CommentDtos
{
    public class CommentKpiDto
    {
        public int TotalCommentCount { get; set; }
        public int PublishedCommentCount { get; set; }
        public int PendingCommentCount { get; set; }
        public double AverageRating { get; set; }
    }
}
