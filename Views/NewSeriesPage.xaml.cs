namespace SeriesTracker.Views;

public partial class NewSeriesPage : ContentPage
{
    public NewSeriesPage()
    {
        InitializeComponent();
    }

    private void ChangeSeasonEnabled(bool checkFlag)
    {
        seasonLabel.IsEnabled = checkFlag;
        seasonEntry.IsEnabled = checkFlag;

    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        ChangeSeasonEnabled(e.Value);
    }
}