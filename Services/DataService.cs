using CommunityToolkit.Maui.Core.Extensions;
using SeriesTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services
{
    public static class DataService
    {
        public static ObservableCollection<Series> GetSearchResults(string queryString, ObservableCollection<Series> series)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            return series.Where(f => f.seriesName.ToLowerInvariant().Contains(normalizedQuery)).ToObservableCollection();
        }
    }
}
