using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorAPIClient.Dtos;

namespace BlazorAPIClient.DataServices
{
    public class GraphqlSpaceXDataService : ISpaceXDataService
    {
        private readonly HttpClient _httpClient;

        public GraphqlSpaceXDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<LaunchDto[]> GetAllLaunches()
        {
            var queryObject = new
            {
                query = @"{ launches { id is_tentative launch_date_local mission_name }}",
                variables = new { }
            };

            var launchQuery = new StringContent(
                JsonSerializer.Serialize(queryObject),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("graphql", launchQuery);

            if (response.IsSuccessStatusCode)
            {
                var gqlData = await JsonSerializer.DeserializeAsync<GqlDataDto>(await response.Content.ReadAsStreamAsync());

                return gqlData.Data.Launches;
            }

            return null;
        }
    }
}