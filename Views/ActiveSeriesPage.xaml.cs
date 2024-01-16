using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using SeriesTracker.Controls.PopUp;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class ActiveSeriesPage : ContentPage
{
    ActiveSeriesPageViewModel activeSeriesPageViewModel;
    public ActiveSeriesPage()
	{
		InitializeComponent();
        this.BindingContext = activeSeriesPageViewModel = new ActiveSeriesPageViewModel(Navigation, this);
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
            activeSeriesPageViewModel.SeriesCount = activeSeriesPageViewModel.SeriesList.Count;
        }
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
        activeSeriesPageViewModel.SeriesCount = activeSeriesPageViewModel.FilterList.Count;
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

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
        activeSeriesPageViewModel.FilterList = activeSeriesPageViewModel.SeriesList.Where(item => item.isFavourite.Equals(true)).ToObservableCollection();
            activeSeriesPageViewModel.SeriesCount = activeSeriesPageViewModel.FilterList.Count;
        seriesCollection.ItemsSource = activeSeriesPageViewModel.FilterList;
        }
        else
        {
            activeSeriesPageViewModel.SeriesCount = activeSeriesPageViewModel.SeriesList.Count;
            seriesCollection.ItemsSource = activeSeriesPageViewModel.SeriesList;
        }
    }
    private void filterExpander_ExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {
        if (filterExpander.IsExpanded)
        {
            filterExpandImage.RotateXTo(180, 200);
        }
        else
            filterExpandImage.RotateXTo(0, 200);
    }
}