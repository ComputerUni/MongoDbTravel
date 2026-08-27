using Travel.Web.DTOs.CommentDtos;

namespace Travel.Web.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<ResultCommentDto>> GetAllAsync();
        Task CreateAsync(CreateCommentDto dto);
        Task<List<ResultCommentDto>> GetByTourIdAsync(string tourId);
        Task<List<ResultCommentDto>> GetByUserIdAsync(string userId);
        Task DeleteAsync(string userId, string commentId);
    }
}
