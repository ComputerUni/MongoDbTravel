using Travel.Web.Entities;
using Travel.Web.DTOs.ReservationDtos;
using System.Text.Json;
using Travel.Web.DTOs.FavoriteDtos;
using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.QuestionDtos;

namespace Travel.Web.Areas.User.Models
{
    public class ProfileViewModel
    {
        public AppUser User { get; set; }
        public List<ResultReservationDto> Reservations { get; set; }
        public List<ResultFavoriteDto> Favorites { get; set; }
        public List<ResultCommentDto> Comments { get; set; }
        public List<ResultQuestionDto> Questions { get; set; }
    }
}