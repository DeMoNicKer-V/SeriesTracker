using System.Diagnostics;

namespace SeriesTracker.Views;

public partial class InfoPage : ContentPage
{
    public InfoPage()
    {
        InitializeComponent();
    }

    private void emailButton_Clicked(object sender, EventArgs e)
    {
        TpLink("mailto:v.shakov@list.ru");
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        TpLink("https://github.com/DeMoNicKer-V");
    }

    private void tgButton_Clicked(object sender, EventArgs e)
    {
        TpLink("https://t.me/Vitek_Dev");
    }

    private void TpLink(string Url)
    {
        try
        {
            Uri uri = new Uri(Url);
            Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Shell.Current.DisplayAlert("Произошла ошибка", "Пожалуйста, проверьте Интернет соединение.", "Ок");
            return;
        }
    }

    private void vkButton_Clicked(object sender, EventArgs e)
    {
        TpLink("https://vk.com/v_shakov");
    }
}