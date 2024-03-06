using System.Diagnostics;

namespace SeriesTracker.Views;

public partial class InfoPage : ContentPage
{
	public InfoPage()
	{
		InitializeComponent();
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://vk.com/v_shakov");
            Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Shell.Current.DisplayAlert("Произошла ошибка", "Пожалуйста, проверьте Интернет соединение.", "Ок");
            return;
        }
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://github.com/DeMoNicKer-V");
            Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Shell.Current.DisplayAlert("Произошла ошибка", "Пожалуйста, проверьте Интернет соединение.", "Ок");
            return;
        }
    }
}