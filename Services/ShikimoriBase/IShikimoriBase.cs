using SeriesTracker.Classes.Shikimori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Abstractions;
using SeriesTracker.Classes;

namespace SeriesTracker.Services.ShikimoriBase
{
    internal interface IShikimoriBase
    {
        Task<GraphQLResponse<AnimeList<ShikimoriAnime>>> GetAnimes();
        Task<GraphQLResponse<AnimeList<ShikimoriAnime>>> GetAnimesByName();
    }
}
