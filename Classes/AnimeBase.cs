﻿using System.Text.Json.Serialization;

namespace SeriesTracker.Classes
{
    public abstract class AnimeBase
    {
        [JsonIgnore] public abstract string Description { get; set; }
        [JsonIgnore] public abstract double Duration { get; set; }
        [JsonIgnore] public abstract int Episodes { get; set; }
        [JsonIgnore] public abstract double Score { get; set; }
        [JsonIgnore] public abstract string StartDate { get; set; }
        [JsonIgnore] public abstract string Title { get; set; }
        [JsonIgnore] public abstract string SubTitle { get; set; }
        [JsonIgnore] public abstract string PictureUrl { get; }
    }

}