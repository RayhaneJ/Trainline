using System.Text;

namespace Trainline.RoomAvailability.Models
{
    public class Schedule
    {
        public string DayOfWeek { get; set; }
        public List<Availability> Availabilities { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var availability in Availabilities)
            {
                var isAvailable = availability.IsAvailable ? 1 : 0;
                builder.Append(isAvailable);
            }

            return builder.ToString();
        }
    }
}
