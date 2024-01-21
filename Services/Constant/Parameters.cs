using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.Constant
{
    internal class Parameters
    {
        public static bool WachedFlag = false; 
        public enum LOAD_PARAMETER
        {
            DEFAULT,
            FILTER,
            FAVORITE
        }
    }
}
