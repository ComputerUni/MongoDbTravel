using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Areas.User.Models
{
    public class TourDetailViewModel
    {
        public ResultTourDto Tour { get; set; }
        public List<ResultCommentDto> Comments { get; set; }
    }
}
