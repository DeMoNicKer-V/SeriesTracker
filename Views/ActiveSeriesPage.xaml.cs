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
        base.OnAppearing();
        activeSeriesPageViewModel.OnAppearing();
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.OldTextValue) & string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            searchbarClearBtn.IsVisible = false;
            OnAppearing();
        }
        else searchbarClearBtn.IsVisible = true;
    }

    private void searchbarClearBtn_Clicked(object sender, EventArgs e)
    {
        searchBar.Text = string.Empty;
    }
}