using Newtonsoft.Json;


namespace SeriesTracker.Classes.Shikimori
{

    public class Anime
    {
        [JsonProperty("russian")] public string RussianName { get; set; }
        public string Name { get; set; }
        public int Episodes  { get; set; }
        public float Score { get; set; }
        [JsonIgnore] public string PosterUrl { get { return poster.OriginalUrl;  }}
        [JsonProperty("poster")] public Poster poster;
        [JsonIgnore] public string ReleaseDate { get { return airedOne.Date; } }
        [JsonProperty("airedOn")] public AiredDate airedOne;
    }
    public class AnimeList
    {
        public IEnumerable<Anime> Animes { get; set; }
    }

}
