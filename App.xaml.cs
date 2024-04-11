using SeriesTracker.Services;
using SeriesTracker.Views;
using System.ComponentModel;
using static SeriesTracker.Services.Constant.SeriesBaseParameters;

namespace SeriesTracker;

public partial class App : Application
{
    private static SeriesService _seriesService;
    public static SeriesService SeriesService
    {
        get
        {
            if (_seriesService == null)
            {
                _seriesService = new SeriesService(Path.Combine(Environment.GetFolderPath(
                   Environment.SpecialFolder.LocalApplicationData), "seriestracker.db"));
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


    private async void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SettingsService.Theme))
        {
            try
            {
                SetTheme();
            }
            catch (Exception ex)
            {
                await ShowErrorAlert(Shell.Current, ex.Message);
                UserAppTheme = AppTheme.Unspecified;
            }
           
        }
    }

    private void SetTheme()
    {
        UserAppTheme = SettingsService.Instance?.Theme != null
                     ? SettingsService.Instance.Theme.AppTheme
                     : AppTheme.Unspecified;
    }
}
