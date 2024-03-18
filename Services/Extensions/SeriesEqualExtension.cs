using SeriesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.Extensions
{
    public abstract class SeriesEqualExtension
    {
        public  class SeriesSyncComparer : IEqualityComparer<Series>
        {
            public bool Equals(Series x, Series y)
            {
                if (x.Equals(y))
                {
                    if (x.ChangedDate == y.ChangedDate)
                    {
                        return true;
                    }
                    y.seriesId = x.seriesId;
                }
                return false;
            }

            public int GetHashCode(Series obj)
            {
                return obj.hiddenSeriesName.GetHashCode();
            }
        }

        public class SeriesExportComparer : IEqualityComparer<Series>
        {
            public bool Equals(Series x, Series y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(Series obj)
            {
                return (obj.hiddenSeriesName).GetHashCode();
            }
        }
    }
}
