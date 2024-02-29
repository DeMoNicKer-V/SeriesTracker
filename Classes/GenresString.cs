using Newtonsoft.Json;

namespace SeriesTracker.Classes.Shikimori
{
    internal class GenresString<T>
    {
        [JsonProperty("genres")] public Genre[] GenreList { get; set; }

        public override string ToString()
        {
            object Type = typeof(T);
            if (Type is Anime)
            {
                return string.Join(", ", GenreList.Select(l => l.Russian));
            }
            else return string.Join(", ", GenreList.Select(l => l.English));
        }
    }
}