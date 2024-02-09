using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeriesTracker.Classes.MAL
{
    public class MALAnime :AnimeBase
    {
        [JsonPropertyName("title")] public override  string Title { get; set; }
        [JsonPropertyName("mean")] public override double Score { get; set; }
        [JsonPropertyName("num_episodes")] public override int Episodes { get; set; }
        [JsonPropertyName("start_date")] public override string StartDate { get; set; }
        [JsonPropertyName("average_episode_duration")] public int SecondDuration { get; set; }

        [JsonIgnore]
        public override double Duration
        {
            get { return SecondDuration > 0 ? Math.Floor(SecondDuration / 60.0) : SecondDuration; }
            set { }
        }
        [JsonPropertyName("synopsis")] public override string Description { get; set; }
        [JsonPropertyName("main_picture")] public PictureInfo Picture { get; set; } = new();
        [JsonIgnore] public override string PictureUrl { get { return Picture != null ? Picture.medium : string.Empty; } }
        [JsonPropertyName("alternative_titles")] public AlternativeTitle AlternativeTitles { get; set; } = new();

        [JsonIgnore] public override string SubTitle { get { return AlternativeTitles.EngTitle != null ? AlternativeTitles.EngTitle : AlternativeTitles.JapTitle;  } set { } }

    }

}
