using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeriesTracker.Classes.MAL
{
    public class PictureInfo
    {
        [JsonPropertyName("medium")] public string medium { get; set; }
        [JsonPropertyName("large")] public string large { get; set; }
    }
}
