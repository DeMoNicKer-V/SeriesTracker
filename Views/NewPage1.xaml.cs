using SeriesTracker.Classes;
using SeriesTracker.Classes.MAL;
using SeriesTracker.Services.MALBase;
using SeriesTracker.Services.ShikimoriBase;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace SeriesTracker.Views;

public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();
	}

    public class AnimeListResult : Base<AnimeListResultItem>
    {

    }
    public class Paging
    {
        public string next { get; set; }
        public string? previous { get; set; }
    }
    public abstract class Base<T1>
    {
        public IEnumerable<T1> data { get; set; }
        public Paging paging { get; set; }
    }

    public class AnimeListResultItem
    {
        public Anime node { get; set; }
    }

    public class Anime 
    {
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("mean")] public double Score { get; set; }
        [JsonPropertyName("num_episodes")] public int Episodes { get; set; }
        [JsonPropertyName("start_date")] public string StartDate { get; set; }
        [JsonPropertyName("average_episode_duration")] public int SecondDuration { get; set; }
        [JsonIgnore]
        public double Duration
        {
            get { return SecondDuration > 0 ? Math.Floor(SecondDuration / 60.0) : SecondDuration; }
            set { }
        }
        [JsonPropertyName("synopsis")] public string Description { get; set; }

        [JsonPropertyName("main_picture")] public PictureInfo Picture { get; set; }
        [JsonPropertyName("alternative_titles")] public AlternativeTitle AlternativeTitle { get; set; }
    }

    public class PictureInfo
    {
        [JsonPropertyName("medium")] public string medium { get; set; }
        [JsonPropertyName("large")] public string large { get; set; }
    }

    public class AlternativeTitle
    {
        [JsonPropertyName("en")] public string EngTitle { get; set; }
        [JsonPropertyName("ja")] public string JaTitle { get; set; }
    }
    private MALBase MALBase;
    public async Task<AnimeListResult> GetAsync() 
	{
        var url = "https://api.myanimelist.net/v2/anime/ranking?ranking_type=all&limit=3&offset=0";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();

        var msg = new HttpRequestMessage(HttpMethod.Get, url);
        msg.Headers.Add("X-MAL-CLIENT-ID", "");
        var res = await client.SendAsync(msg);

        string responseBody = await res.Content.ReadAsStringAsync();
        var content = await res.Content.ReadFromJsonAsync<AnimeListResult>();


        return content;

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var result = await GetAsync();

        var r = result;


    }
}