﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeriesTracker.Classes.MAL
{
    public class AlternativeTitle
    {
        [JsonPropertyName("en")] public string EngTitle { get; set; }
        [JsonPropertyName("ja")] public string JapTitle { get; set; }
    }
}
