using SQLite;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SeriesTracker.Models;

public partial class Series
{
    [PrimaryKey, AutoIncrement]
    public int seriesId
    {
        get; set;
    }
    public int MalId
    {
        get; set;
    } = 0;

    public string seriesName
    {
        get; set;
    }

    public string hiddenSeriesName
    {
        get; set;
    }

    public int seriesDuration
    {
        get; set;
    } = 24;

    public string seriesDescription
    {
        get; set;
    }

    public float seriesRating
    {
        get; set;
    }

    public int currentEpisode
    {
        get; set;
    } = 0;

    public string imagePath
    {
        get; set;
    }

    public int lastEpisode
    {
        get; set;
    } = 10;

    public DateTime releaseDate
    {
        get; set;
    } = DateTime.Now;

    public string addedDate
    {
        get; set;
    }
    public string overDate
    {
        get; set;
    }
    public string ChangedDate
    {
        get; set;
    }

    public bool isOver
    {
        get; set;
    } = false;

    public bool isFavourite
    {
        get; set;
    } = false;

    public (bool IsValid, string? ErrorMessage) Validate()
    {
        if (string.IsNullOrWhiteSpace(seriesName)) 
        {
            return (false, "Название обязательное поле!");
        }
        else if (currentEpisode > lastEpisode || currentEpisode < 0)
        {
            return (false, "Текущая серия должна быть больше последней и больше 0!");
        }
        else if (new[] { lastEpisode, seriesDuration }.Min() <= 0)
        {
            return (false, "Последняя серия и ср. продолжительность должны быть больше 0!");
        }
        else if (releaseDate.Year < 1961)
        {
            return (false, "Некорректная дата!");
        }
        return (true, null);
    }

    [JsonIgnore]
    public string GetFormatDate
    {
        get
        {
            return System.String.Format($"{releaseDate:d MMMM, yyyy} г.");
        }
    }

    [JsonIgnore]
    public string GetAddedDate
    {
        get
        {
            if (!string.IsNullOrEmpty(addedDate))
            {
                return DateTime.Parse(addedDate).ToString("d MMMM, yyyy") + " г.";
            }
            return null;
        }
    }

    [JsonIgnore]
    public string GetOverDate
    {
        get
        {
            if (!string.IsNullOrEmpty(overDate))
            {
                return DateTime.Parse(overDate).ToString("d MMMM, yyyy") + " г.";
            }
            return null;
        }
    }
}