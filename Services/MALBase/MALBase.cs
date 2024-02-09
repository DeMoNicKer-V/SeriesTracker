﻿using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using SeriesTracker.Classes;
using SeriesTracker.Classes.MAL;
using SeriesTracker.Classes.Shikimori;
using System.Net.Http.Json;
using System.Text;
using Xamarin.Google.Crypto.Tink.Prf;

namespace SeriesTracker.Services.MALBase
{
    internal class MALBase
    {
        private static readonly string apiUrl = "https://api.myanimelist.net/v2/anime/";

        public MALBase()
        {
           
        }

        public async Task<AnimeList<MALAnimeItem>> GetAnimes()
        {
            var url = "https://api.myanimelist.net/v2/anime/ranking?ranking_type=all&limit=3&offset=0&fields=title,mean,num_episodes,start_date,average_episode_duration,synopsis,main_picture,alternative_titles";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            var msg = new HttpRequestMessage(HttpMethod.Get, url);
            msg.Headers.Add("X-MAL-CLIENT-ID", APIConfig.ClientId);
            var res = await client.SendAsync(msg);

            string responseBody = await res.Content.ReadAsStringAsync();
            var content = await res.Content.ReadFromJsonAsync<AnimeList<MALAnimeItem>>();


            return content;
        }


        string[] Fields = new string[] { "title","mean","num_episodes","start_date","average_episode_duration","synopsis","main_picture","alternative_titles" };
        public string FormURL()
        {
            string result = apiUrl + String.Format($"?limit=10&offset=1");
            if (Fields.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var x in Fields)
                {
                    sb.Append(x);
                    if (x != Fields.Last()) sb.Append(",");
                }
                result += "&fields=" + sb.ToString();
            }
            return result;
        }
    }
}
