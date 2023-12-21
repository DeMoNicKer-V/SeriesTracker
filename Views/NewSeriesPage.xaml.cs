using AngleSharp;
using AngleSharp.Browser;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using SeriesTracker.Models;
using SeriesTracker.Services;
using SeriesTracker.ViewModels;
using System;
using System.Globalization;
using System.Reflection.Metadata;

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
            if (Convert.ToInt32(seasonEntry.Text) > 0) { seasonCheckBox.IsChecked = true; }
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
    string url = string.Empty;
    private async  void ImageButton_Clicked(object sender, EventArgs e)
    {
        if (!await CheckInternetAccess())
        {
            return;
        }
        webView.Source = "https://myanimelist.net/anime.php";

        webView.IsVisible = true;
    }

    ShikimoriParser shikimoriParser = new ShikimoriParser();
    MALParser malParser = new MALParser();
    Services.IParser parser;

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
    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (!String.IsNullOrWhiteSpace(url))
        {

            switch (url.Contains("shikimori"))
            {
                case true:
                    shikimoriParser.BaseUrl = aaa.Text;
                    await shikimoriParser.ParseDate();
                    parser = shikimoriParser;
                    break;
                case false:
                    malParser.BaseUrl = url;
                    await malParser.ParseDate();
                    parser = malParser;
                    break;
                default:
                    break;
            }


            nameEditor.Text = parser.Name;
            descriptionEditor.Text = parser.Description;
            releaseEntry.Text = parser.ReleaseYear;
            lastEntry.Text = parser.Episodes;
            posterImage.Source = parser.ImagePath;
            webView.IsVisible = false;
            url = string.Empty;
        }
    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {

        if (!await CheckInternetAccess())
        {
            return;
        }
        url = "shikimori";
        await Browser.Default.OpenAsync("https://shikimori.one/animes", BrowserLaunchMode.SystemPreferred);
    }

    private void webView_Navigating(object sender, WebNavigatingEventArgs e)
    {

        url = e.Url;

    }


}