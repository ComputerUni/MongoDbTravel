namespace Travel.Web.DTOs.QuestionDtos
{
    public class ResultQuestionDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string TourId { get; set; }
        public string TourName { get; set; }
        public string Content { get; set; }
        public string? Answer { get; set; }
        public bool IsAnswered { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserFullName { get; set; }
        public string UserInitials { get; set; }
    }
}
