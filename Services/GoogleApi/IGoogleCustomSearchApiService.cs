using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.GoogleApi
{
    public interface IGoogleCustomSearchApiService
    {
        Task<IEnumerable<string>> SearchAsync(string searchPhrase, int pageNumber, int pageSize);
    }
}
