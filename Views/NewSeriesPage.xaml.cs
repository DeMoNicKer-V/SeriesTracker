using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;
using SeriesTracker.Controls.PopUp;
using SeriesTracker.Controls.SearchImageResult;
using SeriesTracker.Models;
using SeriesTracker.ViewModels;
using Color = Microsoft.Maui.Graphics.Color;

namespace SeriesTracker.Views;

public partial class NewSeriesPage : ContentPage
{
    private SearchImageResult imageResultPopUp;

    public NewSeriesPage()
    {
        InitializeComponent();
        BindingContext = new NewSeriesPageViewModel();
        posterImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#919191") });
    }

    public NewSeriesPage(Series series)
    {
        InitializeComponent();
        this.BindingContext = new NewSeriesPageViewModel();

        if (series != null)
        {
            ((NewSeriesPageViewModel)BindingContext).Series = series;
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
        await Navigation.PushAsync(new SeriesListPage(true));
    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        if (!await CheckInternetAccess())
        {
            return;
        }
        await Navigation.PushAsync(new SeriesListPage(false));
    }


    private void RemoveSignsTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            bool isValid = e.NewTextValue.ToCharArray().All(x => char.IsDigit(x));

            ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
        }
    }

    private void durationEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        RemoveSignsTextChanged(sender, e);
    }

    private async Task SearchImages(string name)
    {
        if (!await CheckInternetAccess())
        {
            return;
        }
        imageResultPopUp = new SearchImageResult();
        imageResultPopUp.SearchName = name;
        imageResultPopUp.Page = this;
        var result = await this.ShowPopupAsync(imageResultPopUp);
        if (result is bool boolResult)
        {
            if (boolResult == true)
            {
                SetImageParams(imageResultPopUp.Images.ElementAt(imageResultPopUp.ActiveImage));
            }
        }
    }

    private void SetImageParams(string urlImage)
    {
        Behavior toRemove = posterImage.Behaviors.FirstOrDefault(b => b is IconTintColorBehavior);
        if (toRemove != null)
        {
            posterImage.Behaviors.Remove(toRemove);
        }
        posterImage.Source = urlImage;
        ((NewSeriesPageViewModel)BindingContext).Series.imagePath = urlImage;
        ChangePosterAttributes();
    }

    private async void SelectPosterImage_Tapped(object sender, TappedEventArgs e)
    {
        var displayResult = await this.ShowPopupAsync(new CustomAlert("Поиск изображения", "Искать изображение в интеренте?", "Нет", "Да"));
        if (displayResult is bool boolResult)
        {
            if (boolResult == true)
            {
                if (string.IsNullOrWhiteSpace(nameEditor.Text))
                {
                    await this.ShowPopupAsync(new CustomAlert("Произошла ошибка", "Сначала заполните название сериала", "Закрыть"));
                    return;
                }
                await SearchImages(nameEditor.Text);
                return;
            }
            else
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
    }
}