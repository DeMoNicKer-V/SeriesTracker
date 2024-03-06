using CommunityToolkit.Maui.Core.Platform;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class ActiveSeriesPage : ContentPage
{
    private ActiveSeriesPageViewModel activeSeriesPageViewModel;

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

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            searchbarClearBtn.IsVisible = false;
            activeSeriesPageViewModel.queryText = string.Empty;
        }
        else searchbarClearBtn.IsVisible = true;
    }

    private void SearchBarClearBtn_Clicked(object sender, EventArgs e)
    {
        searchBar.Text = string.Empty;
        searchBar.Unfocus();
        OnAppearing();
    }

    private void SearchBar_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(searchBar.Text)) { searchBar.Unfocus(); return; }
        else 
        {
            SeriesEmptyView.MainText = "�� ������ ������� ������ �� ������� :( ";
            SeriesEmptyView.SubText = "��������� ����� ��� ���?";
        }
    }

    private async void SearchBar_Unfocused(object sender, FocusEventArgs e)
    {
        searchbarClearBtn.IsVisible = false;
        SeriesEmptyView.MainText = "����� ����� '�������' ��������� :( ";
        SeriesEmptyView.SubText = "����� ����� ��� ���������?";
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
        activeSeriesPageViewModel.favoriteFlag = !activeSeriesPageViewModel.favoriteFlag;
        activeSeriesPageViewModel.skip = 0;
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