using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Areas.User.Models
{
    public class TourDetailViewModel
    {
        public ResultTourDto Tour { get; set; }
        public List<ResultCommentDto> Comments { get; set; }
        public List<ResultQuestionDto> Questions { get; set; }
        public List<ResultTourDto> SimilarTours { get; set; }
    }
}
