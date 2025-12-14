using System.Text.Json;
using Trainline.RoomAvailability.Models;
using DayOfWeek = Trainline.RoomAvailability.Models.DayOfWeek;

namespace Trainline.RoomAvailability
{
    public class RoomAvailabilityService
    {
        private readonly IRoomAvailabilityRepository _roomAvailabilityRepository;
        private readonly int _defaultTimeRange = 30;
        private readonly double _initialSlotTime = 00.00;

        public RoomAvailabilityService(IRoomAvailabilityRepository roomAvailabilityRepository)
        {
            _roomAvailabilityRepository = roomAvailabilityRepository;
        }

        public async Task<bool> CheckSlotsAvailability(string room, DayOfWeek dayOfWeek, TimeSpan slots)
        {
            var weekAvailabilities = await _roomAvailabilityRepository.GetAvailabilities();
            var dayAvailabilities = weekAvailabilities.GetAvailabilityForDay(dayOfWeek);

            var requiredSlots = (int)(slots.TotalMinutes / _defaultTimeRange);
            var pattern = new string('1', requiredSlots);

            return Enumerable.Range(0, dayAvailabilities.Length - requiredSlots + 1).Any(i => dayAvailabilities.Substring(i, requiredSlots) == pattern);
        }

        public async Task<RoomAvailabilities> 
            GetAllAvailabilities(string room)
        {
            var weekAvailabilities = await _roomAvailabilityRepository.GetAvailabilities();
            var availabilitiesByDays = weekAvailabilities.GetAll();

            var schedules = new List<Schedule>();

            foreach(var day in availabilitiesByDays)
            {
                List<Availability> availabilities = GenerateSlotsFromDayAvailabilities(day.Availability);

                var schedule = new Schedule
                {
                    DayOfWeek = day.Day.ToString(),
                    Availabilities = availabilities
                };

                schedules.Add(schedule);
            }

            var roomAvailabilities = new RoomAvailabilities
            {
                Name = room,
                Schedules =  schedules
            };

            return roomAvailabilities;
        }

        public async Task<RoomAvailabilities> GetAvailabilitiesByBusinessDayOfWeek(string room, DayOfWeek dayOfWeek)
        {
            var weekAvailabilities = await _roomAvailabilityRepository.GetAvailabilities();
            var dayAvailabilities = weekAvailabilities.GetAvailabilityForDay(dayOfWeek);

            List<Availability> availabilities = GenerateSlotsFromDayAvailabilities(dayAvailabilities);

            var roomAvailabilities = new RoomAvailabilities
            {
                Name = room,
                Schedules = new List<Schedule>
                {
                    new Schedule
                    {
                        DayOfWeek = dayOfWeek.ToString(),
                        Availabilities = availabilities
                    }
                }
            };

            return roomAvailabilities;
        }

        private List<Availability> GenerateSlotsFromDayAvailabilities(string dayAvailabilities)
        {
            var availabilities = new List<Availability>();
            var startAvailability = TimeSpan.FromHours(_initialSlotTime);

            foreach (var rangeAvailability in dayAvailabilities)
            {
                var availability = new Availability
                {
                    Range = startAvailability,
                    IsAvailable = rangeAvailability == '1'
                };

                startAvailability = startAvailability.Add(TimeSpan.FromMinutes(_defaultTimeRange));

                availabilities.Add(availability);
            }

            return availabilities;
        }
    }
}
