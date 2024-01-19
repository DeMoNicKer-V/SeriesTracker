using SQLite;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SeriesTracker.Models;

public partial class Series
{
    [PrimaryKey, AutoIncrement, JsonIgnore]
    public int seriesId
    {
        get; set;
    }

    public string seriesName
    {
        get; set;
    }

    public string hiddenSeriesName
    {
        get; set;
    }

    public int seriesSeason
    {
        get; set;
    } = 0;

    public string seriesDescription
    {
        get; set;
    }

    public float seriesRating
    {
        get; set;
    }

    public int startEpisode
    {
        get; set;
    } = 1;

    public int currentEpisode
    {
        get; set;
    } = 1;

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
        else if (startEpisode < 0 & startEpisode > lastEpisode)
        {
            return (false, "Первая серия должна быть меньше, чем последняя!");
        }
        else if (lastEpisode < startEpisode)
        {
            return (false, "Последняя серия должна быть больше или равна стартовой!");
        }
        else if (currentEpisode < startEpisode | currentEpisode > lastEpisode)
        {
            return (false, "Текущая серия не должа выходить за пределы стартовой и последней серий!");
        }
        else if (new[] { startEpisode, lastEpisode, currentEpisode, seriesSeason }.Min() < 0)
        {
            return (false, "Все числовые значения не должны отрицательными!");
        }
        else if (releaseDate.Year < 1961)
        {
            return (false, "Некорректный год выхода!");
        }
        return (true, null);
    }

    [JsonIgnore]
    public string GetFormatDate
    {
        get
        {
                return System.String.Format($"{releaseDate:M} {releaseDate:yyyy} г.");
        }
    }

    [JsonIgnore]
    public string GetSubDescription
    {
        get
        {
            if (seriesSeason > 0)
            {
                return $"Сезон {seriesSeason}-й ({lastEpisode} эпизодов)";
            }
            return $"{lastEpisode} эпизодов";
        }
    }

    [JsonIgnore]
    public string getIsRated
    {
        get
        {
            if (seriesRating == 0)
            {
                return "(?)";
            }
            return seriesRating.ToString();
        }
    }

    [JsonIgnore]
    public string getAddedDate
    {
        get
        {
            if (!string.IsNullOrEmpty(addedDate))
            {
                return System.String.Format($"{DateTime.Parse(addedDate):D}");
            }
            return null;
        }
    }

    [JsonIgnore]
    public string getOverDate
    {
        get
        {
            if (!string.IsNullOrEmpty(overDate))
            {
                return System.String.Format($"{DateTime.Parse(overDate):D}");
            }
            return null;
        }
    }

    [JsonIgnore]
    public string getFomattedEpisodes
    {
        get
        {
            if (currentEpisode == startEpisode)
                return System.String.Format($"{0} из {lastEpisode} эп.");
            else return System.String.Format($"{currentEpisode} из {lastEpisode} эп.");

        }
    }

    [JsonIgnore]
    public string getFormatDate
    {
        get
        {
            if (addedDate != null)
            {
                return System.String.Format($"Дата добавления: {DateTime.Parse(addedDate):f}");
            }
            return addedDate;
        }
    }
}