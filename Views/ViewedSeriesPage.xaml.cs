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
}