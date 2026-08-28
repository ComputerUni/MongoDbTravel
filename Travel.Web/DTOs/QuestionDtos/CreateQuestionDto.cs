namespace Travel.Web.DTOs.QuestionDtos
{
    public class CreateQuestionDto
    {
        public string UserId { get; set; }
        public string TourId { get; set; }
        public string Content { get; set; }
    }
}
