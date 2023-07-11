using SeriesTracker.Services;
using SeriesTracker.Views;

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
        Routing.RegisterRoute(nameof(ActiveSeriesPage), typeof(ActiveSeriesPage));
        Routing.RegisterRoute(nameof(NewSeriesPage), typeof(NewSeriesPage));
        Routing.RegisterRoute(nameof(EditSeriesPage), typeof(EditSeriesPage));
        MainPage = new AppShell();
	}
}
