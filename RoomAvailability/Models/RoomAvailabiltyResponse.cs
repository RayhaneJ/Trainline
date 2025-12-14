using System.Text.Json.Serialization;

namespace Trainline.RoomAvailability.Models
{
    internal class RoomAvailabiltyResponse
    {
        [JsonPropertyName("availability")]
        public WeekAvailabilities Availabilities { get; set; } = new();
    }
}
