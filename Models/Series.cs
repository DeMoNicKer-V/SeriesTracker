﻿using SQLite;
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

    public int seriesSeason
    {
        get; set;
    } = 0;

    public string seriesDescription
    {
        get; set;
    } = "Описание отсутствует";

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

    public int releaseYear
    {
        get; set;
    } = DateTime.Now.Year;

    public string addedDate
    {
        get; set;
    }

    public bool isOver
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
        else if (releaseYear < 1961)
        {
            return (false, "Некорректный год выхода!");
        }
        return (true, null);
    }


    [JsonIgnore]
    public string GetTitleDescription
    {
        get
        {
            if (seriesSeason > 0)
            {
                return $"Сезон {seriesSeason}-й, год выхода - {releaseYear}";
            }
            return $"Год выхода - {releaseYear}";
        }
    }

    [JsonIgnore]
    public string GetSubDescription
    {
        get
        {
            if (seriesSeason > 0)
            {
                return $"Сезон {seriesSeason}-й ({lastEpisode} эп.), {releaseYear} год";
            }
            return $"{lastEpisode} эп., {releaseYear} год";
        }
    }

    [JsonIgnore]
    public string getAddedDate
    {
        get
        {
            if (addedDate != null)
            {
                return System.String.Format($"{DateTime.Parse(addedDate):d}");
            }
            return addedDate;
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