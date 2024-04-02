using SeriesTracker.Classes.Shikimori;
using GraphQL;
using SeriesTracker.Classes;

namespace SeriesTracker.Services.ShikimoriBase
{
    internal interface IShikimoriBase
    {
        Task<GraphQLResponse<ShikimoriAnimeList>> GetAnimes();
        Task<GraphQLResponse<ShikimoriAnimeList>> GetAnimesByName();
    }
}
