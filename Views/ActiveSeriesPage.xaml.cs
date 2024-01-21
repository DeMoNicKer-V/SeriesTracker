using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using SeriesTracker.Controls.PopUp;
using SeriesTracker.ViewModels;
using static SeriesTracker.Services.Constant.Parameters;

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
        if (activeSeriesPageViewModel.loadParameter == LOAD_PARAMETER.FILTER)
        {
            activeSeriesPageViewModel.loadParameter = LOAD_PARAMETER.DEFAULT;
            OnAppearing();
        }
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

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        activeSeriesPageViewModel.favoriteflag = !activeSeriesPageViewModel.favoriteflag;
        OnAppearing();
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

    private void filterExpandImage_Clicked(object sender, EventArgs e)
    {
        if (!filterExpander.IsExpanded)
        {
            filterExpandImage.RotateXTo(180, 200);
            filterExpander.IsExpanded = true;
        }
        else
        {
            filterExpandImage.RotateXTo(0, 200);
            filterExpander.IsExpanded = false;
        }
    }
}