namespace Trainline.RoomAvailability.Models
{
    public class Availability
    {
        public TimeSpan Range { get; set; }
        public bool IsAvailable { get; set; }
    }
}
