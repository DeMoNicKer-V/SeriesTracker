using Google.Apis.Services;
using SeriesTracker.Services.Exceptions;

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
            Google.Apis.CustomSearchAPI.v1.Data.Search responseResult = new();

            try
            {
                responseResult = await listRequest.ExecuteAsync();
            }
            catch (Exception)
            {
                throw new LimitQuotaCustomSeachException();
            }
   
            if (responseResult == null)
            {
                throw new ArgumentNullException(nameof(responseResult));
            }

            var urlItems = responseResult.Items?.Select(x => new string(x.Link)) ?? new List<string>();
            return urlItems;
        }
    }
}
