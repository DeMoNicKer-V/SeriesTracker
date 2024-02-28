using System.Text;

namespace SeriesTracker.Services.MALBase
{
    internal class MALRequest
    {
        public string Search { get; set; } = String.Empty;
        public int Limit { get; set; } = 100;
        public int Offset { get; set; } = 0;
        private string[] Fields { get; set; } = Consts.Fields;

        public string FormURL()
        {
            string result = string.Empty;
            switch (string.IsNullOrEmpty(Search))
            {
                case true:
                    result = Consts.AnimeListRanking + String.Format($"?ranking_type=all&nsfw=1&limit={Limit}&offset={Offset}");
                    break;

                case false:
                    result = Consts.AnimeList + String.Format($"?q={Search}&nsfw=1&limit={Limit}&offset={Offset}");
                    break;
            }
            if (Fields.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var x in Fields)
                {
                    sb.Append(x);
                    if (x != Fields.Last()) sb.Append(",");
                }
                result += "&fields=" + sb.ToString();
            }
            return result;
        }
    }
}