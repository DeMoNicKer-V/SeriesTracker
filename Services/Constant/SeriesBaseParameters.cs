using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.Constant
{
    public static class SeriesBaseParameters
    {
        public static bool WachedFlag = false;
        public static readonly string FilePath = Environment.GetFolderPath(
                   Environment.SpecialFolder.LocalApplicationData);

        private static int skipItem;
        public static int SkipItem
        {
            get => skipItem;
            set
            {
                if (skipItem < 0) { skipItem = 0; }
                else { skipItem = value; }
            }
        }
        public static bool FavoriteFlag { get; set; }

        public static string QueryText { get; set; } = string.Empty;
    }
}
