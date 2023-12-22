using AngleSharp;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using SeriesTracker.Models;
using SeriesTracker.Services;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class NewSeriesPage : ContentPage
{
    private MALParser malParser = new MALParser();

    private Services.IParser parser;

    private ShikimoriParser shikimoriParser = new ShikimoriParser();

    private string url = string.Empty;

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
            if (Convert.ToInt32(seasonEntry.Text) > 0) { seasonCheckBox.IsChecked = true; }
            if (!string.IsNullOrWhiteSpace(series.imagePath))
            {
                ChangePosterAttributes();
                posterImage.Source = series.imagePath;
            }
        }
    }

    public Series Series
    {
        get; set;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (!String.IsNullOrWhiteSpace(url))
        {
            saveBtn.IsEnabled = false;
            posterImage.Source = string.Empty;
            ChangePosterAttributes();
            switch (url.Contains("shikimori"))
            {
                case true:
                    shikimoriParser.BaseUrl = aaa.Text;
                    aaa.Text = string.Empty;
                    siteEntry.IsVisible = false;
                    await shikimoriParser.ParseDate();
                    parser = shikimoriParser;
                    break;

                case false:
                    malParser.BaseUrl = aaa.Text;
                    aaa.Text = string.Empty;
                    siteEntry.IsVisible = false;
                    await malParser.ParseDate();
                    parser = malParser;
                    break;

                default:
                    break;
            }
            saveBtn.IsEnabled = true;
            nameEditor.Text = parser.Name;
            descriptionEditor.Text = parser.Description;
            releaseEntry.Text = parser.ReleaseYear;
            lastEntry.Text = parser.Episodes;
            posterImage.Source = parser.ImagePath;
            ((NewSeriesPageViewModel)BindingContext).Series.imagePath = parser.ImagePath;
            url = string.Empty;
        }
    }

    private void ChangePosterAttributes()
    {
        tipPosterLabel.IsVisible = false;
        posterBorder.HeightRequest = 220;
        posterImage.Aspect = Aspect.Fill;
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

    private async Task<bool> CheckInternetAccess()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;

        if (accessType != NetworkAccess.Internet)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = "Отсутствует интернет подключение";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
            return false;
        }
        return true;
    }

    private void descriptionEditor_Focused(object sender, FocusEventArgs e)
    {
        descriptionUnderline.HeightRequest = 2;
    }

    private void descriptionEditor_Unfocused(object sender, FocusEventArgs e)
    {
        descriptionUnderline.HeightRequest = 1;
    }

    private async void descriptionExpander_ExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {
        if (descriptionExpander.IsExpanded)
        {
            descriptionCaption.Text = "Скрыть описание";
            await descriptionImage.RotateXTo(180, 200);
        }
        else
        {
            descriptionCaption.Text = "Открыть описание";
            await descriptionImage.RotateXTo(0, 200);
        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        if (!await CheckInternetAccess())
        {
            return;
        }
        url = "myanimelist";
        siteEntry.IsVisible = true;
        await Browser.Default.OpenAsync("https://myanimelist.net/anime.php", BrowserLaunchMode.SystemPreferred);
    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        if (!await CheckInternetAccess())
        {
            return;
        }
        url = "shikimori";
        siteEntry.IsVisible = true;
        await Browser.Default.OpenAsync("https://shikimori.one/animes", BrowserLaunchMode.SystemPreferred);
    }

    private void ImageButton_Clicked_2(object sender, EventArgs e)
    {
        siteEntry.IsVisible = false;
    }

    private void nameEditor_Focused(object sender, FocusEventArgs e)
    {
        nameUnderline.HeightRequest = 2;
    }

    private void nameEditor_Unfocused(object sender, FocusEventArgs e)
    {
        nameUnderline.HeightRequest = 1;
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

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            PickOptions options = new()
            {
                PickerTitle = "Пожалуйста, выбирите файл с данными!",
                FileTypes = FilePickerFileType.Png,
            };
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                posterImage.Source = result.FullPath;
                ((NewSeriesPageViewModel)BindingContext).Series.imagePath = result.FullPath;
            }
            ChangePosterAttributes();
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }
    }
}