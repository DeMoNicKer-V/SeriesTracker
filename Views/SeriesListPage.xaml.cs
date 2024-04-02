using CommunityToolkit.Maui.Core.Platform;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class SeriesListPage : ContentPage
{
    private readonly SeriesListPageViewModel seriesListPageViewModel;
    public SeriesListPage()
    {
        InitializeComponent();
        this.BindingContext = seriesListPageViewModel = new SeriesListPageViewModel(Navigation);
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            searchbarClearBtn.IsVisible = false;
        }
        else searchbarClearBtn.IsVisible = true;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await seriesListPageViewModel.OnAppearing();
    }

    private void searchbarClearBtn_Clicked(object sender, EventArgs e)
    {
        searchBar.Unfocus();
        if (!string.IsNullOrWhiteSpace(seriesListPageViewModel.RequestText))
        {
            seriesListPageViewModel.RequestText = string.Empty;
            seriesListPageViewModel.CurrentPage = 1;
            seriesListPageViewModel.OffSet = 0;
            OnAppearing();
        }
        searchBar.Text = string.Empty;

    }

    private void searchBar_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(searchBar.Text)) { searchBar.Unfocus(); return; }
        else
        {
            SeriesEmptyView.MainText = "По вашему запросу ничего не найдено :( ";
            SeriesEmptyView.SubText = "Попытаете удачу еще раз?";
        }
    }

    private async void searchBar_Unfocused(object sender, FocusEventArgs e)
    {
        searchbarClearBtn.IsVisible = false;
        SeriesEmptyView.MainText = "Получаем данные...";
        SeriesEmptyView.SubText = "Это может занять некоторое время. Пожалуйста, подождите!";
        await searchImage.RotateTo(0, 200);
        await searchBar.HideKeyboardAsync(CancellationToken.None);
    }

    private async void searchBar_Focused(object sender, FocusEventArgs e)
    {
        searchbarClearBtn.IsVisible = true;
        await searchImage.RotateTo(90, 200);
    }


}