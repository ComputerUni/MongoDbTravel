using Travel.Web.Entities;
using Travel.Web.DTOs.ReservationDtos;
using System.Text.Json;
using Travel.Web.DTOs.FavoriteDtos;

namespace Travel.Web.Areas.User.Models
{
    public class ProfileViewModel
    {
        public AppUser User { get; set; }
        public List<ResultReservationDto> Reservations { get; set; }
        public List<ResultFavoriteDto> Favorites { get; set; }
    }
}