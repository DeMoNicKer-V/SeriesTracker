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
            var year = doc.QuerySelector("#content > table > tbody > tr > td.borderClass > div > div:nth-child(19)").TextContent.ToString();
            DateTime bb = new DateTime();
            try
            {

                var a = year.Split(":\n ").Last();
                a = a.Split(" to").First().Trim();
                bb = DateTime.Parse(a);

            }
            catch
            {
                year = doc.QuerySelector("#content > table > tbody > tr > td.borderClass > div > div:nth-child(20)").TextContent.ToString(); var a = year.Split(":\n ").Last();
                a = a.Split(" to").First().Trim();
                bb = DateTime.Parse(a);
            }

            var description = doc.QuerySelector("#content > table > tbody > tr > td:nth-child(2) > div.rightside.js-scrollfix-bottom-rel > table > tbody > tr:nth-child(1) > td > p").TextContent.ToString();
            var image = doc.QuerySelector("#content > table > tbody > tr > td.borderClass > div > div:nth-child(1) > a > img").GetAttribute("data-src").ToString();


            var episodes = doc.QuerySelector("#curEps").TextContent.ToString();

            ReleaseYear = bb.Year.ToString();
           ImagePath = image;
           Name = title;
            Description = description;
           Episodes = episodes;
        }
    }
}
