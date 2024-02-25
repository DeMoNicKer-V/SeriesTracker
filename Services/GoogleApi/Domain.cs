using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.GoogleApi
{
    public record GoogleSearchResultItem(string Headline, string Description, string Url);
}
