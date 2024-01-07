using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Platform;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class ViewedSeriesPage : ContentPage
{
    ViewedSeriesPageViewModel viewedSeriesPageViewModel;
    public ViewedSeriesPage()
	{
		InitializeComponent();
        this.BindingContext = viewedSeriesPageViewModel = new ViewedSeriesPageViewModel(Navigation);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewedSeriesPageViewModel.OnAppearing();
    }
    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.OldTextValue) & string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            searchbarClearBtn.IsVisible = false;
            seriesCollection.ItemsSource = viewedSeriesPageViewModel.SeriesList;


        }
        else searchbarClearBtn.IsVisible = true;
    }

    private void searchbarClearBtn_Clicked(object sender, EventArgs e)
    {
        searchBar.Text = string.Empty;
    }

    private void searchBar_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(searchBar.Text)) return;
        var normalizedQuery = searchBar.Text?.ToLower() ?? "";
        viewedSeriesPageViewModel.FilterList = viewedSeriesPageViewModel.SeriesList.Where(item => item.seriesName.ToLowerInvariant().Contains(normalizedQuery)).ToObservableCollection();
        seriesCollection.ItemsSource = viewedSeriesPageViewModel.FilterList;
    }

    private async void searchBar_Unfocused(object sender, FocusEventArgs e)
    {
        await searchBar.HideKeyboardAsync(CancellationToken.None);
    }
}