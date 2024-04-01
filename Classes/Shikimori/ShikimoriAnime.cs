using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SeriesTracker.Classes.Shikimori
{
    public class ShikimoriAnime : AnimeBase
    {
        [JsonProperty("airedOn")] public AiredDate airedOne = new();
        [JsonProperty("poster")] public Poster poster = new();
        [JsonProperty("genres")] public Genre[] Genre { get; set; }


        [JsonIgnore]
        public override string Genres
        {
            get
            { return string.Join(", ", Genre.Select(l => l.Russian)); }
            set { }
        }

        [JsonIgnore] public override string Description { get { return string.IsNullOrEmpty(description) ? description : Regex.Replace(description, @" ?\[.*?\]", " "); } set { } }
        [JsonProperty("duration")] public override double Duration { get; set; }
        [JsonProperty("episodes")] public override int Episodes { get; set; }

        [JsonProperty("kind")] public override string Kind { get; set; }
        [JsonProperty("status")] public string StatuscInfo { get; set; }
        [JsonIgnore] public override string Status
        {
            get { return ConvertStatusToDefault(StatuscInfo); }
            set { }
        }
        [JsonIgnore] public override string PictureUrl { get { return poster.OriginalUrl; } }

        [JsonIgnore]
        public override string Rating
        {
            get { return ConvertRatingToImageName(RatingInfo); }
            set { }
        }

        [JsonProperty("rating")] public string RatingInfo { get; set; }
        [JsonProperty("score")] public override double Score { get; set; }

        [JsonIgnore]
        public override string StartDate
        {
            get
            {
                if (airedOne.Date != null)
                {
                    return DateTime.Parse(airedOne.Date).ToString("d MMMM, yyyy") + " г.";
                }
                return airedOne.Date;
            }
            set { }
        }
        [JsonProperty("name")] public override string SubTitle { get; set; }
        [JsonProperty("russian")] public override string Title { get; set; }
        [JsonProperty("description")] private string description { get; set; }

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

                case "r_plus":
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
                case "anons":
                    return "Анонс";

                case "ongoing":
                    return "Онгоинг";

                case "released":
                    return "Вышло";

                case null: return "Неизвестно";
                default:
                    return statusName;
            }
        }
    }
}