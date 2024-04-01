using SeriesTracker.Classes.Shikimori;
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
        [JsonPropertyName("start_date")] public string StartDateInfo { get; set; }

        [JsonIgnore]
        public override string StartDate
        {
            get
            {
                if (StartDateInfo != null)
                {
                    return DateTime.Parse(StartDateInfo).ToString("d MMMM, yyyy") + " г.";
                }
                return StartDateInfo;
            }
            set { }
        }
        [JsonPropertyName("average_episode_duration")] public int SecondDuration { get; set; }

        [JsonPropertyName("genres")] public Genre[] Genre { get; set; }


        [JsonIgnore]
        public override string Genres
        {
            get
            { return string.Join(", ", Genre.Select(l => l.English)); }
            set { }
        }

        [JsonIgnore]
        public override double Duration
        {
            get { return SecondDuration > 0 ? Math.Floor(SecondDuration / 60.0) : SecondDuration; }
            set { }
        }
        [JsonPropertyName("synopsis")] public override string Description { get; set; }
        [JsonPropertyName("rating")] public string RatingInfo { get; set; }

        [JsonIgnore] public override string Rating {
            get { return ConvertRatingToImageName(RatingInfo); }
            set { }
        }

        [JsonPropertyName("status")] public string StatusInfo { get; set; }
        
        [JsonIgnore]
        public override string Status
        {
            get { return ConvertStatusToDefault(StatusInfo); }
            set { }
        }
        [JsonPropertyName("media_type")] public override string Kind { get; set; }
        [JsonPropertyName("main_picture")] public PictureInfo Picture { get; set; } = new();
        [JsonIgnore] public override string PictureUrl { get { return Picture != null ? Picture.large : string.Empty; } }
        [JsonPropertyName("alternative_titles")] public AlternativeTitle AlternativeTitles { get; set; } = new();
        [JsonIgnore] public override string SubTitle { get { return AlternativeTitles.EngTitle != null ? AlternativeTitles.EngTitle : AlternativeTitles.JapTitle;  } set { } }

        protected override string ConvertRatingToImageName(string ratingName)
        {
            switch (ratingName)
            {
                case "pg_13":
                    return "pg13";

                case "pg":
                    return "pg";

                case "g":
                    return "g0";

                case "r":
                    return "r16";

                case "r+":
                    return "rplus";

                case null: return "none";
                default:
                    return ratingName;
            }
        }
        protected override string ConvertStatusToDefault(string statusName)
        {
            switch (statusName)
            {
                case "not_yet_aired":
                    return "Анонсировано";

                case "currently_airing":
                    return "Онгоинг";

                case "finished_airing":
                    return "Вышло";

                case null: return "Неизвестно";
                default:
                    return statusName;
            }
        }
    }

}
