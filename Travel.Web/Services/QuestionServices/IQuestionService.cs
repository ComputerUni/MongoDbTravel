using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.DTOs.ReservationDtos;

namespace Travel.Web.Services.QuestionServices
{
    public interface IQuestionService
    {
        Task CreateAsync(CreateQuestionDto dto);
        Task<ResultQuestionDto> GetByIdAsync(string id);
        Task<List<ResultQuestionDto>> GetAllAsync();
        Task<List<ResultQuestionDto>> GetByTourIdAsync(string tourId);
        Task<List<ResultQuestionDto>> GetByUserIdAsync(string userId);
        Task<List<ResultQuestionDto>> GetUnansweredAsync();
        Task AnswerAsync(string questionId, string answer);
        Task<QuestionKpiDto> GetQuestionKpiAsync();
    }
}
