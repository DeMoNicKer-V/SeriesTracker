﻿using SeriesTracker.Classes.Shikimori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace SeriesTracker.Services.ShikimoriBase
{
    internal interface IShikimoriBase
    {
        Task<GraphQLResponse<AnimeList>> GetAnimes();
        Task<GraphQLResponse<AnimeList>> GetAnimesByName();
    }
}