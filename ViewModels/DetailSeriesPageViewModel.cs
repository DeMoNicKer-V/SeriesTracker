﻿using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using SeriesTracker.Services.SyncJournal;
using SeriesTracker.Views;

#if ANDROID

using static Android.Media.MediaRouter;

#endif

namespace SeriesTracker.ViewModels
{
    public partial class DetailSeriesPageViewModel : BaseSeriesModel
    {
        public Command CloseCommand { get; }

        public Command DeleteCommand { get; }

        public Command DetachCommand { get; }

        public Command EditCommand { get; }

        public Command BackCommand { get; }
        public Command ChangeRatingCommand { get; }

        public DetailSeriesPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            BackCommand = new Command(OnBackCommand);
            EditCommand = new Command(OnEditCommand);
            DeleteCommand = new Command(OnDeleteCommand);
            DetachCommand = new Command(OnDetachCommand);
            ChangeRatingCommand = new Command(OnChangeRating);
            Series = new Series();
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        [RelayCommand]
        public async Task ChangeFavoriteStatus()
        {
            if (!CheckSeries(Series)) { return; }
            Series.isFavourite = !Series.isFavourite;
            await App.SeriesService.AddUpdateSeriesAsync(Series);
        }

        public async void OnChangeRating()
        {
            if (!CheckSeries(Series)) { return; }
            await App.SeriesService.AddUpdateSeriesAsync(Series);
        }

        [RelayCommand]
        public async Task EditEpisode(string Text)
        {
            if (!CheckSeries(Series)) { return; }
            if (Convert.ToInt32(Text) > Series.lastEpisode || Convert.ToInt32(Text) < 0)
            {
                return;
            }
            Series.currentEpisode = Convert.ToInt32(Text);
            await App.SeriesService.AddUpdateSeriesAsync(Series);
        }

        private async void OnDeleteCommand()
        {
            if (!CheckSeries(Series)) { return; }
            new Journal(new DeleteItem(Series.SyncUid)).JournalToJson();
            await App.SeriesService.DeleteSeriesAsync(Series.seriesId);
            await Shell.Current.GoToAsync("..");
        }

        private bool CheckSeries(Series series)
        {
            if (series is null) return false;
            else return true;
        }

        private async void OnDetachCommand()
        {
            if (!CheckSeries(Series)) { return; }
            if (!Series.isOver)
            {
                Series.currentEpisode = Series.lastEpisode;
                Series.overDate = DateTime.Now.ToString();
            }
            else
            {
                Series.currentEpisode = 1;
                Series.overDate = string.Empty;
            }
            Series.isOver = !Series.isOver;
            await App.SeriesService.AddUpdateSeriesAsync(Series);
            await Shell.Current.GoToAsync("..");
        }

        private async void OnEditCommand()
        {
            await Navigation.PushAsync(new NewSeriesPage(Series));
        }

        private async void OnBackCommand()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}