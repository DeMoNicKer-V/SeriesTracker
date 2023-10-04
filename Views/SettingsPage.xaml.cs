using SeriesTracker.Services;

namespace SeriesTracker.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
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
            Shell.Current.DisplayAlert("��������� ������", "����������, ��������� �������� ����������.", "��");
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
            Shell.Current.DisplayAlert("��������� ������", "����������, ��������� �������� ����������.", "��");
            return;
        }
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Preferences.Set("AppTheme", SettingsService.Instance.Theme.AppTheme.ToString());
    }
}