using Trainline.RoomAvailability.Models;

namespace Trainline.RoomAvailability
{
    public interface IRoomAvailabilityRepository
    {
        Task<WeekAvailabilities> GetAvailabilities();
    }
}
