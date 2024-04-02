﻿using SeriesTracker.Classes.Shikimori;
using GraphQL;
using SeriesTracker.Classes;

namespace SeriesTracker.Services.ShikimoriBase
{
    internal interface IShikimoriBase
    {
        Task<GraphQLResponse<AnimeList<ShikimoriAnime>>> GetAnimes();
        Task<GraphQLResponse<AnimeList<ShikimoriAnime>>> GetAnimesByName();
    }
}
