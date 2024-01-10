using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Platform;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class ActiveSeriesPage : ContentPage
{
    ActiveSeriesPageViewModel activeSeriesPageViewModel;
    public ActiveSeriesPage()
	{
		InitializeComponent();
        this.BindingContext = activeSeriesPageViewModel = new ActiveSeriesPageViewModel(Navigation);
    }

    protected override void OnAppearing()
    {
        searchBar.Text = string.Empty;
        base.OnAppearing();
        activeSeriesPageViewModel.OnAppearing();
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            searchbarClearBtn.IsVisible = false;
            seriesCollection.ItemsSource = activeSeriesPageViewModel.SeriesList;


        }
        else searchbarClearBtn.IsVisible = true;
    }

    private void searchbarClearBtn_Clicked(object sender, EventArgs e)
    {
        searchBar.Text = string.Empty;
        searchBar.Unfocus();
    }

    private void searchBar_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(searchBar.Text)) { searchBar.Unfocus(); return; }
        var normalizedQuery = searchBar.Text?.ToLower() ?? "";
        activeSeriesPageViewModel.FilterList = activeSeriesPageViewModel.SeriesList.Where(item => item.seriesName.ToLowerInvariant().Contains(normalizedQuery)).ToObservableCollection();
        seriesCollection.ItemsSource = activeSeriesPageViewModel.FilterList;
    }

    private async void searchBar_Unfocused(object sender, FocusEventArgs e)
    {
       await searchImage.RotateTo(0, 200);
       await searchBar.HideKeyboardAsync(CancellationToken.None);
    }

    private async void searchBar_Focused(object sender, FocusEventArgs e)
    {
        await searchImage.RotateTo(90, 200);
    }
}