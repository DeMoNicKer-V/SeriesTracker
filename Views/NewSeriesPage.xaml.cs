using SeriesTracker.Models;
using SeriesTracker.ViewModels;
using System.Numerics;

namespace SeriesTracker.Views;

public partial class NewSeriesPage : ContentPage
{
    public Series Series
    {
        get; set;
    }
    public NewSeriesPage()
    {
        InitializeComponent();
        BindingContext = new NewSeriesPageViewModel();
    }

    public NewSeriesPage(Series series)
    {
        InitializeComponent();
        this.BindingContext = new NewSeriesPageViewModel();

        if (series != null)
        {
            ((NewSeriesPageViewModel)BindingContext).Series = series;
        }
    }

    private void ChangeSeasonEnabled(bool checkFlag)
    {
        seasonLabel.IsEnabled = checkFlag;
        seasonEntry.IsEnabled = checkFlag;
        if (checkFlag == false)
        {
            seasonEntry.Text = "0";
        }

    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        ChangeSeasonEnabled(e.Value);
    }

    private void startEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue.Length > 0)
        {
            currentEntry.Text = e.NewTextValue;
        }
    }

    private void currentEntry_TextChanged(object sender, TextChangedEventArgs e)
    {

        if (!string.IsNullOrEmpty(currentEntry.Text))
        {
            if (Convert.ToInt32(currentEntry.Text) < Convert.ToInt32(startEntry.Text))
            {
                currentEntry.Text = startEntry.Text;
                return;
            }
            else if (Convert.ToInt32(currentEntry.Text) > Convert.ToInt32(lastEntry.Text))
            {
                currentEntry.Text = lastEntry.Text;
                return;
            }
        }
    }

    private void lastEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(lastEntry.Text))
        {
            if (Convert.ToInt32(startEntry.Text) > Convert.ToInt32(lastEntry.Text))
            {
                lastEntry.Text = startEntry.Text;
                return;
            }
        }
    }
}