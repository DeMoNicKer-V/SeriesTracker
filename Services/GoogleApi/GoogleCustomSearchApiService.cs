using Google.Apis.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.GoogleApi
{
    internal class GoogleCustomSearchApiService : IGoogleCustomSearchApiService
    {
        private readonly GoogleCustomSearchApiConfiguration _configuration;

        public GoogleCustomSearchApiService()
        {
            _configuration = new GoogleCustomSearchApiConfiguration();
        }

        public async Task<IEnumerable<string>> SearchAsync(string searchPhrase, int pageNumber, int pageSize)
        {
            var searchService = new Google.Apis.CustomSearchAPI.v1.CustomSearchAPIService(new BaseClientService.Initializer
            {
                ApiKey = _configuration.ApiKey
            });

            var listRequest = searchService.Cse.List();
            listRequest.Cx = _configuration.Cx;
            listRequest.Q = searchPhrase;
            listRequest.SearchType = Google.Apis.CustomSearchAPI.v1.CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Num = pageSize < 10 ? pageSize : 10;
            listRequest.Start = (pageNumber - 1) * pageSize;
            listRequest.ImgSize = Google.Apis.CustomSearchAPI.v1.CseResource.ListRequest.ImgSizeEnum.XLARGE;


            var results = await listRequest.ExecuteAsync();
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }

            var urlItems = results.Items?.Select(x => new string(x.Link)) ?? new List<string>();
            return urlItems;
        }
    }
}
