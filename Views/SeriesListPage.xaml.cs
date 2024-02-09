using CommunityToolkit.Maui.Core.Platform;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using SeriesTracker.Classes.Shikimori;
using SeriesTracker.Models;
using SeriesTracker.Services.ShikimoriBase;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class SeriesListPage : ContentPage
{
    SeriesListPageViewModel seriesListPageViewModel;
    public SeriesListPage(bool what)
    {
        InitializeComponent();
        this.BindingContext = seriesListPageViewModel = new SeriesListPageViewModel(Navigation, what);
    }


    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            searchbarClearBtn.IsVisible = false;

        }
    }
    protected override void OnAppearing()
    {
        searchBar.Text = string.Empty;
        base.OnAppearing();
        seriesListPageViewModel.OnAppearing();
    }

    private void searchbarClearBtn_Clicked(object sender, EventArgs e)
    {
        seriesListPageViewModel.quaryText = string.Empty;
        searchBar.Text = string.Empty;
        searchBar.Unfocus();
        OnAppearing();

    }

    private void searchBar_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(searchBar.Text)) { searchBar.Unfocus(); return; }
    }

    private async void searchBar_Unfocused(object sender, FocusEventArgs e)
    {
        searchbarClearBtn.IsVisible = false;
        await searchImage.RotateTo(0, 200);
        await searchBar.HideKeyboardAsync(CancellationToken.None);
    }

    private async void searchBar_Focused(object sender, FocusEventArgs e)
    {
        searchbarClearBtn.IsVisible = true;
        await searchImage.RotateTo(90, 200);
    }


}