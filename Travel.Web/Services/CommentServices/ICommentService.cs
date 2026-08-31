using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.ReservationDtos;

namespace Travel.Web.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<ResultCommentDto>> GetAllAsync();
        Task CreateAsync(CreateCommentDto dto);
        Task<List<ResultCommentDto>> GetByTourIdAsync(string tourId);
        Task<List<ResultCommentDto>> GetByUserIdAsync(string userId);
        Task DeleteAsync(string userId, string commentId);
        Task UpdateAsync(string commentId, string content, int rating);
        Task UpdateRatingAsync(string commentId, int rating);
        Task ApproveAsync(string commentId);
        Task MarkAsSpamAsync(string commentId);
        Task<CommentKpiDto> GetCommentKpiAsync();
    }
}
