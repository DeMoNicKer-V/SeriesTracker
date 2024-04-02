using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using SeriesTracker.Classes;
using SeriesTracker.Classes.MAL;
using SeriesTracker.Classes.Shikimori;
using System.Net.Http.Json;
using System.Text;

namespace SeriesTracker.Services.MALBase
{
    internal class MALBase
    {
        private HttpClient _httpClient;
        public MALBase()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.BaseAddress = new Uri(Consts.baseUrl);
            _httpClient.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", APIConfig.ClientId);
        }

        public async Task<MALAnimeList> GetAnimes(MALRequest request)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(request.FormURL());
            return await response.Content.ReadFromJsonAsync<MALAnimeList>();
        }
    }
}
