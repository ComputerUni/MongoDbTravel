using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.DashboardDtos;
using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.DTOs.ReservationDtos;

namespace Travel.Web.Services.DashboardServices
{
    public interface IDashboardService
    {
        Task<DashboardKpiDto> GetKpiAsync();
        Task<List<MonthlyReservationDto>> GetMonthlyReservationsAsync(string range);
        Task<List<PopularTourDto>> GetPopularToursAsync();
        Task<List<ResultReservationDto>> GetRecentReservationAsync();
        Task<List<ResultCommentDto>> GetRecentCommentsAsync();
        Task<List<ResultQuestionDto>> GetPendingQuestionsAsync();
    }
}
