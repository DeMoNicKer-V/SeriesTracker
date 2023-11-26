using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class DetailSeriesPage : ContentPage
{
    public DetailSeriesPage()
	{
		InitializeComponent();

    }
    readonly double sliderIncrement = 0.5;

    // The value for the slider we will be using.
    double sliderCorrectValue;

    // The actual method. If the Slider object was instantiated in C#, read below.
    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 0.5;
        Slider slider = (Slider)sender;
        slider.Value = Math.Round(e.NewValue / step) * step;
    }
}