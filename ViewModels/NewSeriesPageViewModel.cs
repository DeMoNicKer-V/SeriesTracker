﻿using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Android.Media.MediaRouter;

namespace SeriesTracker.ViewModels
{
    public partial class NewSeriesPageViewModel : BaseSeriesModel
    {
        public NewSeriesPageViewModel()
        {
            Series = new Series();
        }

        [RelayCommand]
        public async void SaveSeries()
        {
            var newSeries = Series;
            if (newSeries is null)
                return;

            var (isValid, errorMessage) = newSeries.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Validation Error", errorMessage, "Ok");
                return;
            }
            await App.SeriesService.AddUpdateSeriesAsync(newSeries);

            await Shell.Current.GoToAsync("..");
        }
    }
}