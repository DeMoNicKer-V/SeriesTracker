using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Classes
{
    public class AnimeList<T>
    {
        public IEnumerable<T> Animes { get; set; }
    }
}
