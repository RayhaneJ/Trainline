
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;
using Trainline.RoomAvailability.Models;

namespace Trainline.RoomAvailability
{
    public class GitHubRoomAvailabilityRepository : IRoomAvailabilityRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _gitHubUrlAvailability;

        public GitHubRoomAvailabilityRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _gitHubUrlAvailability = configuration["Github:AvailabilityUrl"] ?? throw new InvalidOperationException("Availability url is not set");
        }

        public async Task<WeekAvailabilities> GetAvailabilities()
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };

            var response = await _httpClient.GetFromJsonAsync<RoomAvailabiltyResponse>(_gitHubUrlAvailability, options);
            return response?.Availabilities ?? throw new InvalidOperationException("No availability was returned");
        }
    }
}
