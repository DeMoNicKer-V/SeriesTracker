﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeriesTracker.Classes.MAL
{
    public class MALAnimeList
    {
        [JsonPropertyName("data")] public MALAnimeItem[] Animes { get; set; }
    }
}
