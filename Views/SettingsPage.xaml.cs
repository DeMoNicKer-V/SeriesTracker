using CommunityToolkit.Maui.Storage;
using SeriesTracker.Services;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class SettingsPage : ContentPage
{
    private SettingsPageViewModel settingsPageViewModel;
    public SettingsPage(IFileSaver fileSaver)
    {
        InitializeComponent();
        this.BindingContext = settingsPageViewModel = new SettingsPageViewModel(fileSaver);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await settingsPageViewModel.OnAppearing();
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Preferences.Set("AppTheme", SettingsService.Instance.Theme.AppTheme.ToString());
    }

    private void Picker_SelectedIndexChanged_1(object sender, EventArgs e)
    {
        Preferences.Set("SyncType", SettingsService.Instance.Sync.SyncId);
    }
}