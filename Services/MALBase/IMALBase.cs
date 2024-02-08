using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Abstractions;
using SeriesTracker.Classes;
using SeriesTracker.Classes.MAL;

namespace SeriesTracker.Services.MALBase
{
    internal interface IMALBase
    {
        Task<AnimeList<MALAnime>> GetAnimes();
    }
}
