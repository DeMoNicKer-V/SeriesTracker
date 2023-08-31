﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

    public int lastEpisode
    {
        get; set;
    } = 10;

    public int currentEpisode
    {
        get; set;
    } = 1;
    public int seriesSeason
    {
        get; set;
    } = 0;

    public int releaseYear
    {
        get; set;
    } = 2023;

    public (bool IsValid, string? ErrorMessage) Validate()
    {
        if (string.IsNullOrWhiteSpace(seriesName))
        {
            return (false, "Название обязательное поле.");
        }
        else if (startEpisode < 0 & startEpisode > lastEpisode) 
        {
            return (false, "Первая серия должна быть меньше, чем последняя");
        }
        else if (lastEpisode < startEpisode)
        {
            return (false, "Последняя серия должна быть больше или равна первой");
        }
        return (true, null);
    }
}

