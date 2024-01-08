using AngleSharp.Dom;
using AngleSharp;
using SeriesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services
{
    internal class MALParser : IParser
    {
        public MALParser(string url)
        {
            BaseUrl = url;
        }
        public MALParser()
        {
         
        }
        public string BaseUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public string ImagePath { get; set; }
        public string ReleaseYear { get; set; }
        public string Episodes { get; set; }

        public async  Task ParseDate() {

            var config = Configuration.Default
.WithDefaultLoader();

            //Create a new context for evaluating webpages with the given config
            IBrowsingContext context = BrowsingContext.New(config);
            var doc = await context.OpenAsync(BaseUrl);

            var title = "";
            try
            {
                title = doc.QuerySelector("#contentWrapper > div:nth-child(1) > div > div.h1-title > div > p").TextContent.ToString();
            }
            catch (Exception ex) { title = doc.QuerySelector("#contentWrapper > div:nth-child(1) > div > div.h1-title > div > h1 > strong").TextContent.ToString(); }
            var year = doc.QuerySelectorAll("#content > table > tbody > tr > td.borderClass > div > div.spaceit_pad");
            string releaseDate = string.Empty, episodes = string.Empty;
            foreach (var item in year)
            {
                if (!string.IsNullOrEmpty(item.TextContent))
                {
                    if (item.TextContent.Contains("Episodes"))
                    {
                        episodes = item.TextContent.Split("Episodes:").Last().Trim();
                    }
                    if (item.TextContent.Contains("Aired"))
                    {
                        releaseDate = item.TextContent.Split("Aired:").Last();
                        releaseDate = releaseDate.Split("to").First().Trim();
                        break;
                    }
                }
            }


            var description = doc.QuerySelector("#content > table > tbody > tr > td:nth-child(2) > div.rightside.js-scrollfix-bottom-rel > table > tbody > tr:nth-child(1) > td > p").TextContent.ToString();
            var image = doc.QuerySelector("#content > table > tbody > tr > td.borderClass > div > div:nth-child(1) > a > img").GetAttribute("data-src").ToString();


            ReleaseYear = releaseDate;
           ImagePath = image;
           Name = title;
            Description = description;
           Episodes = episodes;
        }
    }
}
