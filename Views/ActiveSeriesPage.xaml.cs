using CommunityToolkit.Maui.Core.Platform;
using SeriesTracker.ViewModels;
using static SeriesTracker.Services.Constant.SeriesBaseParameters;

namespace SeriesTracker.Views;

public partial class ActiveSeriesPage : ContentPage
{
    private readonly ActiveSeriesPageViewModel activeSeriesPageViewModel;

    public ActiveSeriesPage()
    {
        InitializeComponent();
        BindingContext = activeSeriesPageViewModel = new ActiveSeriesPageViewModel(Navigation, this);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await activeSeriesPageViewModel.OnAppearing();
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            searchbarClearBtn.IsVisible = false;
        }
        else searchbarClearBtn.IsVisible = true;
    }

    private void SearchBarClearBtn_Clicked(object sender, EventArgs e)
    {
        searchBar.Unfocus();
        if (!string.IsNullOrWhiteSpace(QueryText))
        {
            QueryText = string.Empty;
            OnAppearing();
        }
        searchBar.Text = string.Empty;
    }

    private void SearchBar_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(searchBar.Text)) { searchBar.Unfocus(); return; }
        else
        {
            SeriesEmptyView.MainText = "По вашему запросу ничего не найдено :( ";
            SeriesEmptyView.SubText = "Попытаете удачу еще раз?";
        }
    }

    private async void SearchBar_Unfocused(object sender, FocusEventArgs e)
    {
        searchbarClearBtn.IsVisible = false;
        SeriesEmptyView.MainText = "Здесь самую 'малость' пустовато :( ";
        SeriesEmptyView.SubText = "Может стоит это исрпавить?";
        await searchImage.RotateTo(0, 200);
        await searchBar.HideKeyboardAsync(CancellationToken.None);
    }

    private async void SearchBar_Focused(object sender, FocusEventArgs e)
    {
        searchbarClearBtn.IsVisible = true;
        await searchImage.RotateTo(90, 200);
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        FavoriteFlag = !FavoriteFlag;
        OnAppearing();
    }

    private void FilterExpandImage_Clicked(object sender, EventArgs e)
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