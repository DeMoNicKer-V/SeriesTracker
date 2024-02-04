﻿using SeriesTracker.Services;
using SeriesTracker.Views;
using System.ComponentModel;

namespace SeriesTracker;

public partial class App : Application
{
    public static SeriesService _seriesService;
    public static SeriesService SeriesService
    {
        get
        {
            if (_seriesService == null)
            {
                _seriesService = new SeriesService(Path.Combine(Environment.GetFolderPath(
                   Environment.SpecialFolder.LocalApplicationData), "time_is_money.db"));
            }
            return _seriesService;

        }
    }

    public App()
	{
		InitializeComponent();
        MainPage = new AppShell();
            
        SetTheme();
        Routing.RegisterRoute("NewSeriesPage", typeof(NewSeriesPage));
        // subscribe to changes in the settings
        SettingsService.Instance.PropertyChanged += OnSettingsPropertyChanged;
    }


    private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SettingsService.Theme))
        {
            SetTheme();
        }
    }

    private void SetTheme()
    {
        UserAppTheme = SettingsService.Instance?.Theme != null
                     ? SettingsService.Instance.Theme.AppTheme
                     : AppTheme.Unspecified;
    }
}
