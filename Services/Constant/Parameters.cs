using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.Constant
{
    public static class Parameters
    {
        public static bool WachedFlag = false;
        public static readonly string FilePath = Environment.GetFolderPath(
                   Environment.SpecialFolder.LocalApplicationData);
    }
}
