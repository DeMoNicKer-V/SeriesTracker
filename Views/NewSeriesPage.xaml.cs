using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;
using SeriesTracker.Controls.CustomPopUp;
using SeriesTracker.Controls;
using SeriesTracker.Models;
using SeriesTracker.ViewModels;
using Color = Microsoft.Maui.Graphics.Color;
using static SeriesTracker.Services.Constant.SeriesBaseParameters;

namespace SeriesTracker.Views;

public partial class NewSeriesPage : ContentPage
{
    private readonly NewSeriesPageViewModel newSeriesPageViewModel;
    public NewSeriesPage()
    {
        InitializeComponent();
        BindingContext = newSeriesPageViewModel = new NewSeriesPageViewModel();
        posterImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#919191") });
    }

    public NewSeriesPage(Series series)
    {
        InitializeComponent();
        BindingContext = newSeriesPageViewModel = new NewSeriesPageViewModel();

        if (series != null)
        {
            newSeriesPageViewModel.Series = series;
            if (Convert.ToInt32(durationEntry.Text) > 0) { durationCheckBox.IsChecked = true; }
            if (!string.IsNullOrWhiteSpace(series.imagePath))
            {
                SetImageParams(series.imagePath);
            }
            else { posterImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#919191") }); }
        }
    }

    private void ChangePosterAttributes()
    {
        tipPosterLabel.IsVisible = false;
        posterBorder.HeightRequest = 230;
        posterImage.Aspect = Aspect.Fill;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        durationEntry.IsEnabled = e.Value;
        if (e.Value == false)
        {
            durationEntry.Text = "24";
        }
    }

    private static async Task<bool> CheckInternetAccess()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;

        if (accessType != NetworkAccess.Internet)
        {
            await ShowToast("Отсутствует интернет подключение");
            return false;
        }
        return true;
    }


    private async void DescriptionExpander_ExpandedChanged(object sender, ExpandedChangedEventArgs e)
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

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        if (!await CheckInternetAccess())
        {
            return;
        }
        await Navigation.PushAsync(new SeriesListPage());
    }


    private void RemoveSignsTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            bool isValid = e.NewTextValue.ToCharArray().All(x => char.IsDigit(x));

            ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
        }
    }

    private void DurationEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        RemoveSignsTextChanged(sender, e);
    }


    private void SetImageParams(string urlImage)
    {
        Behavior toRemove = posterImage.Behaviors.FirstOrDefault(b => b is IconTintColorBehavior);
        if (toRemove != null)
        {
            posterImage.Behaviors.Remove(toRemove);
        }
        posterImage.Source = urlImage;
        newSeriesPageViewModel.Series.imagePath = urlImage;
        ChangePosterAttributes();
    }

    private async void SelectPosterImage_Tapped(object sender, TappedEventArgs e)
    {
                PickOptions options = new()
                {
                    PickerTitle = "Пожалуйста, выбирите изображение для сериала!",
                    FileTypes = FilePickerFileType.Images,
                };
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    SetImageParams(result.FullPath);
                } 
    }
}