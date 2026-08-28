using System.Text.Json.Serialization;
using Travel.Web.Entities.Enums;

namespace Travel.Web.DTOs.ReservationDtos
{
    public class UpdateReservationStatusDto
    {
        public string Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ReservationStatus Status { get; set; }
    }
}
