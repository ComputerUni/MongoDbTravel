namespace Travel.Web.DTOs.QuestionDtos
{
    public class QuestionKpiDto
    {
        public int TotalQuestionCount { get; set; }
        public int AnsweredQuestionCount { get; set; }
        public int PendingQuestionCount { get; set; }
        public double ResponseRate { get; set; }
    }
}
