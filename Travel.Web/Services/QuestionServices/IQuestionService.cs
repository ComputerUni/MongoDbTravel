using Travel.Web.DTOs.QuestionDtos;

namespace Travel.Web.Services.QuestionServices
{
    public interface IQuestionService
    {
        Task CreateAsync(CreateQuestionDto dto);
        Task<List<ResultQuestionDto>> GetByTourIdAsync(string tourId);
        Task<List<ResultQuestionDto>> GetByUserIdAsync(string userId);
        Task<List<ResultQuestionDto>> GetUnansweredAsync();
        Task AnswerAsync(string questionId, string answer);
    }
}
