using Newtonsoft.Json;

namespace SeriesTracker.Classes.Shikimori
{
    public class Anime
    {
        [JsonProperty("airedOn")] public AiredDate airedOne;
        [JsonProperty("poster")] public Poster poster;
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("episodes")] public int Episodes { get; set; }
        [JsonProperty("episodesAired")] public int EpisodesAired { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonIgnore] public string PosterUrl { get { return poster.OriginalUrl; } }

        [JsonIgnore]
        public string ReleaseDate
        {
            get
            {
                if (airedOne.Date != null)
                {
                    return DateTime.Parse(airedOne.Date).ToString("d MMMM, yyyy") + " г.";
                }
                return airedOne.Date;
            }
        }
        [JsonProperty("russian")] public string RussianName { get; set; }
        [JsonProperty("score")] public float Score { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
    }

    public class AnimeList
    {
        public IEnumerable<Anime> Animes { get; set; }
    }
}