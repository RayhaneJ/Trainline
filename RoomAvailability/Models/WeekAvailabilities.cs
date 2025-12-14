using System.Text.Json.Serialization;

namespace Trainline.RoomAvailability.Models
{
    public class WeekAvailabilities
    {
        [JsonPropertyName("monday")]
        public string Monday { get; set; } = string.Empty;

        [JsonPropertyName("tuesday")]
        public string Tuesday { get; set; } = string.Empty;

        [JsonPropertyName("wednesday")]
        public string Wednesday { get; set; } = string.Empty;

        [JsonPropertyName("thursday")]
        public string Thursday { get; set; } = string.Empty;

        [JsonPropertyName("friday")]
        public string Friday { get; set; } = string.Empty;

        public string GetAvailabilityForDay(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => Monday,
                DayOfWeek.Tuesday => Tuesday,
                DayOfWeek.Wednesday => Wednesday,
                DayOfWeek.Thursday => Thursday,
                DayOfWeek.Friday => Friday,
                _ => string.Empty,
            };
        }

        public IEnumerable<(DayOfWeek Day, string Availability)> GetAll()
        {
            yield return (DayOfWeek.Monday, Monday);
            yield return (DayOfWeek.Tuesday, Tuesday);
            yield return (DayOfWeek.Wednesday, Wednesday);
            yield return (DayOfWeek.Thursday, Thursday);
            yield return (DayOfWeek.Friday, Friday);
        }
    }
}
