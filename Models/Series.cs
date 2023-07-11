using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SeriesTracker.Models;
public partial class Series
{
    [PrimaryKey, AutoIncrement]
    public int seriesId
    {
        get; set;
    }

    public string seriesName
    {
        get; set;
    }

    public int startEpisode
    {
        get; set;
    } = 1;

    public int endEpisode
    {
        get; set;
    }

    public int currentEpisode
    {
        get; set;
    } = 1;
}
