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
}