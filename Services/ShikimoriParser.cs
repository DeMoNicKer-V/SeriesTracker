using AngleSharp.Dom;
using AngleSharp;
using SeriesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Text;

namespace SeriesTracker.Services
{
    internal class ShikimoriParser : IParser
    {
        public ShikimoriParser()
        {

        }
        public ShikimoriParser(string url)
        {
            BaseUrl = url;
        }
        public string BaseUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string ReleaseYear { get; set; }
        public string Episodes { get; set; }

        public async Task ParseDate()
        {

            var config = Configuration.Default
.WithDefaultLoader();

            //Create a new context for evaluating webpages with the given config
            IBrowsingContext context = BrowsingContext.New(config);
            var doc = await context.OpenAsync(BaseUrl);

            var title = doc.QuerySelector("#animes_show > section > div > header > h1").TextContent.ToString().Split("/").First().Trim();
            var year = "";
            try
            {
                year = doc.QuerySelector("#animes_show > section > div > div.menu-slide-outer.x199 > div > div > div:nth-child(1) > div.b-db_entry > div.c-about > div > div.c-info-left > div.block > div > div:nth-child(4) > div > div.value > span.b-tooltipped.dotted.mobile").GetAttribute("Title");
            }
            catch { year = doc.QuerySelector("#animes_show > section > div > div.menu-slide-outer.x199 > div > div > div:nth-child(1) > div.b-db_entry > div.c-about > div > div.c-info-left > div.block > div > div:nth-child(5) > div > div.value").TextContent.ToString(); }

            var description = doc.QuerySelector("#animes_show > section > div > div.menu-slide-outer.x199 > div > div > div:nth-child(1) > div.b-db_entry > div.c-description > div.block > div > div.text > div").TextContent.ToString();
            var image = doc.QuerySelector("#animes_show > section > div > div.menu-slide-outer.x199 > div > div > div:nth-child(1) > div.b-db_entry > div.c-image > div.cc.block > div.c-poster > div > picture > img").GetAttribute("src").ToString();


            var episodes = doc.QuerySelector("#animes_show > section > div > div.menu-slide-outer.x199 > div > div > div:nth-child(1) > div.b-db_entry > div.c-about > div > div.c-info-left > div.block > div > div:nth-child(2) > div > div.value").TextContent.ToString();
            var last = episodes.Split("/").Last().Trim();
            ReleaseYear = DateTransform(year);
            ImagePath = image;
            Name = title;
            Description = description;
            Episodes = last;
        }
        private string DateTransform(string text)
        {
            text = text.ToLower();
            text = text.Split("с ").Last();
            text = text.Split("по").First().Trim();
            return text;
        }
    }
}
