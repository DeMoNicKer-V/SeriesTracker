using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.MALBase
{
    public class Consts
    {
        public static readonly string baseUrl = "https://api.myanimelist.net/v2/";
        public const string AnimeList = "anime";
        public const string AnimeListRanking = "anime/ranking";
        public static readonly string[] Fields = { 
            "title", 
            "mean", 
            "num_episodes",
            "status",
            "rating",
            "media_type",
            "start_date", 
            "average_episode_duration", 
            "synopsis", 
            "main_picture", 
            "alternative_titles" 
        };
    }
}
