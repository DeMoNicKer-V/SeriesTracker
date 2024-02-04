using AngleSharp;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
using SeriesTracker.Models;
using SeriesTracker.Services;
using SeriesTracker.ViewModels;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;

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

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            switch (url.Contains("shikimori"))
            {
                case true:
                    if (!aaa.Text.Contains(url))
                    {
                        

                        var toast = Toast.Make("Неправильная ссылка", ToastDuration.Short, 14);

                        await toast.Show(cancellationTokenSource.Token);
                        return;
                    }
                    saveBtn.IsEnabled = false;
                    posterImage.Source = string.Empty;
                    ChangePosterAttributes();

                    shikimoriParser.BaseUrl = aaa.Text;
                    aaa.Text = string.Empty;
                    siteEntry.IsVisible = false;
                    await shikimoriParser.ParseDate();
                    parser = shikimoriParser;
                    break;

                case false:
                    if (!aaa.Text.Contains(url))
                    {


                        var toast = Toast.Make("Неправильная ссылка", ToastDuration.Short, 14);

                        await toast.Show(cancellationTokenSource.Token);
                        return;
                    }
                    saveBtn.IsEnabled = false;
                    posterImage.Source = string.Empty;
                    ChangePosterAttributes();
                    malParser.BaseUrl = aaa.Text;
                    aaa.Text = string.Empty;
                    siteEntry.IsVisible = false;
                    await malParser.ParseDate();
                    parser = malParser;
                    break;

                default:
                    break;
            }
            releaseDatePicker.Date = DateTime.Parse(parser.ReleaseYear);
            saveBtn.IsEnabled = true;
            nameEditor.Text = parser.Name;
            descriptionEditor.Text = parser.Description;
            
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
            await descriptionEditor.HideKeyboardAsync(CancellationToken.None);
        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        if (!await CheckInternetAccess())
        {
            return;
        }
        url = "https://myanimelist.net/anime/";
        siteEntry.IsVisible = true;
        await Browser.Default.OpenAsync("https://myanimelist.net/anime.php", BrowserLaunchMode.SystemPreferred);
    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        if (!await CheckInternetAccess())
        {
            return;
        }
        /* url = "https://shikimori.one/animes/";
         siteEntry.IsVisible = true;
         await Browser.Default.OpenAsync("https://shikimori.one/animes", BrowserLaunchMode.SystemPreferred);*/
        await Navigation.PushAsync(new SeriesListPage());
    }

    private void ImageButton_Clicked_2(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(aaa.Text))
        {
            aaa.Text = string.Empty;
        }
        else siteEntry.IsVisible = false;
    }

    private void nameEditor_Focused(object sender, FocusEventArgs e)
    {
        nameUnderline.HeightRequest = 2;
    }

    private void nameEditor_Unfocused(object sender, FocusEventArgs e)
    {
        nameUnderline.HeightRequest = 1;
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
            bool displayResult = await DisplayAlert("Поиск изображения", "Искать изображение в интеренте?", "Да", "Нет");
            if (displayResult == true)
            {
                if (string.IsNullOrWhiteSpace(nameEditor.Text))
                {
                    await DisplayAlert("Пусто", "Для поиска заполните название сериала", "Оk");
                    return;
                }
               string name = nameEditor.Text.Replace(" ", "%20") + " постер";
               await Task.Run(async () => await Browser.Default.OpenAsync("https://yandex.by/images/search?text=" + name, BrowserLaunchMode.SystemPreferred));
            }
            PickOptions options = new()
            {
                PickerTitle = "Пожалуйста, выбирите изображение для сериала!",
                FileTypes = FilePickerFileType.Images,
            };
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                posterImage.Source = result.FullPath;
                ((NewSeriesPageViewModel)BindingContext).Series.imagePath = result.FullPath;
                ChangePosterAttributes();
            }
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }
    }

    private void currentEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        { 
            currentEntry.HasError = true;
        }
        else currentEntry.HasError = false;
    }

    private void lastEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            lastEntry.HasError = true;
        }
        else lastEntry.HasError = false;
    }

    private void startEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            startEntry.HasError = true;
        }
        else startEntry.HasError = false;
    }
}