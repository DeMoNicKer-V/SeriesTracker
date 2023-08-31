using SeriesTracker.Models;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class NewSeriesPage : ContentPage
{
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

    public Series Series
    {
        get; set;
    }

    private void ChangeSeasonEnabled(bool checkFlag)
    {
        seasonLabel.IsEnabled = checkFlag;
        seasonEntry.IsEnabled = checkFlag;
        if (checkFlag == false)
        {
            seasonEntry.Text = "0";
        }
        else { seasonEntry.Text = "1"; }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        ChangeSeasonEnabled(e.Value);
    }

    private void currentEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        RemoveSignsTextChanged(sender, e);
    }

    private void currentEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(currentEntry.Text))
        {
            currentEntry.Text = startEntry.Text;
        }
        else
        if (!Enumerable.Range(Convert.ToInt32(startEntry.Text), Convert.ToInt32(lastEntry.Text))
            .Contains(Convert.ToInt32(currentEntry.Text)))
        {
            currentEntry.Text = startEntry.Text;
            return;
        }
    }

    private void lastEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        RemoveSignsTextChanged(sender, e);
    }

    private void lastEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(lastEntry.Text))
        {
            lastEntry.Text = startEntry.Text;
        }
        else if (!string.IsNullOrEmpty(lastEntry.Text))
        {
            if (Convert.ToInt32(startEntry.Text) > Convert.ToInt32(lastEntry.Text))
            {
                lastEntry.Text = startEntry.Text;
                return;
            }
        }
    }

    private void releaseEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        RemoveSignsTextChanged(sender, e);
    }

    private void releaseEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(releaseEntry.Text) | releaseEntry.Text.Length < 4)
        {
            releaseEntry.Text = DateTime.Now.Year.ToString();
        }
    }

    private void RemoveSignsTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            bool isValid = e.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers

            ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
        }
    }

    private void seasonEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        RemoveSignsTextChanged(sender, e);
    }

    private void startEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        RemoveSignsTextChanged(sender, e);
    }

    private void startEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(startEntry.Text))
        {
            startEntry.Text = "0";
        }
        else
        {
            currentEntry.Text = startEntry.Text;

            if (Convert.ToInt32(startEntry.Text) > Convert.ToInt32(lastEntry.Text))
            {
                lastEntry.Text = startEntry.Text;
                return;
            }
        }
    }
}