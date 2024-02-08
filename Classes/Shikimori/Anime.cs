using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SeriesTracker.Classes.Shikimori
{
    public class Anime: AnimeBase
    {
        [JsonProperty("airedOn")] public AiredDate airedOne = new();
        [JsonProperty("poster")] public Poster poster = new();
        [JsonProperty("description")] private string description { get; set; }
        [JsonIgnore] public override string Description { get { return string.IsNullOrEmpty(description) ? description : Regex.Replace(description, @" ?\[.*?\]", " "); } set { } }
        [JsonProperty("duration")] public override double Duration { get; set; }
        [JsonProperty("episodes")] public override int Episodes { get; set; }
        [JsonProperty("episodesAired")] private int episodesAired { get; set; }
        [JsonIgnore] public int EpisodesAired { get { return episodesAired == 0 ? Episodes : episodesAired; } set { } }
        [JsonProperty("name")] public override string SubTitle { get; set; }
        [JsonIgnore] public override string PictureUrl { get { return poster.OriginalUrl; } }

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
        [JsonProperty("russian")] public override string Title { get; set; }
        [JsonProperty("score")] public override double Score { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
    }
}