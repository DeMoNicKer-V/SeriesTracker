using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services
{
    internal interface IParser
    {
        public string BaseUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string ReleaseYear { get; set; }
        public string Episodes { get; set; }
    }
}
