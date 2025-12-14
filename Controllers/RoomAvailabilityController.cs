using Microsoft.AspNetCore.Mvc;
using Trainline.RoomAvailability;
using Trainline.RoomAvailability.Models;
using DayOfWeek = Trainline.RoomAvailability.Models.DayOfWeek;

namespace Trainline.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomAvailabilityController : ControllerBase
    {
        private readonly ILogger<RoomAvailabilityController> _logger;
        private readonly RoomAvailabilityService _roomAvailabilityService;

        public RoomAvailabilityController(ILogger<RoomAvailabilityController> logger, RoomAvailabilityService roomAvailabilityService)
        {
            _logger = logger;
            _roomAvailabilityService = roomAvailabilityService;
        }

        [HttpGet("{room}")]
        public async Task<IActionResult> GetAllAvailabilities(string room)
        {
            return Ok(await _roomAvailabilityService.GetAllAvailabilities(room));
        }

        [HttpGet("{room}/dayOfWeek/{dayOfWeek}")]
        public async Task<IActionResult> GetAvailabilitiesByBusinessDayOfWeek(string room, [FromRoute] string dayOfWeek)
        {
            if (!Enum.TryParse(dayOfWeek, out DayOfWeek day))
            {
                return BadRequest("Invalid day of week");
            }

            if(day == DayOfWeek.Sunday || day == DayOfWeek.Saturday)
            {
                return BadRequest("Only business day are supported");
            }

            return Ok(await _roomAvailabilityService.GetAvailabilitiesByBusinessDayOfWeek(room, day));
        }

        [HttpGet("{room}/check-availability")]
        public async Task<IActionResult> CheckSlotsAvailability(string room, [FromQuery] DayOfWeek dayOfWeek, [FromQuery] int duration)
        {
            if(duration <= 0 || duration %30 != 0)
            {
                return BadRequest("Duration must be within a 30 min range ");
            }

            if(duration > 480)
            {
                return BadRequest("Duration must be less than 8 hours");
            }

            return Ok(await _roomAvailabilityService.CheckSlotsAvailability(room, dayOfWeek, TimeSpan.FromMinutes(duration)));
        }
    }
}
