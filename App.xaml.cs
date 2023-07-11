using SeriesTracker.Services;

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
	}
}
