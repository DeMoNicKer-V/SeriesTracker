using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeriesTracker.Classes.Shikimori
{
    public class Genre
    {
        [JsonProperty("id")] public long Id { get; set; }
        [JsonProperty("russian")] public string Russian { get; set; }
    }
}
