using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.Firebase
{
    public class FirebaseSettings
    {
        public string AppSecret { get; set; }
        public string BaseUrl { get; set; }

        public FirebaseSettings(string appSecret, string baseUrl) 
        { 
            AppSecret = appSecret;
            BaseUrl = baseUrl;
        }
    }
}
