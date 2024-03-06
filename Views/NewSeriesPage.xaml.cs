using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;
using SeriesTracker.Controls.SearchImageResult;
using SeriesTracker.Models;
using SeriesTracker.Services.GoogleApi;
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
        posterBorder.HeightRequest = 220;
        posterImage.Aspect = Aspect.Fill;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        durationEntry.IsEnabled = e.Value;
        if (e.Value == false)
        {
            durationEntry.Text = "24";
        }
        //durationEntry.Style = (Style)Application.Current.Resources["BasicEntryStyle"];
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
            if (boolResult)
            {
                SetImageParams(imageResultPopUp.Images.ElementAt(imageResultPopUp.ActiveImage));
            }
            else
            {
                return;
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

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            bool displayResult = await DisplayAlert("Поиск изображения", "Искать изображение в интеренте?", "Да", "Нет");
            if (displayResult == true)
            {
                if (string.IsNullOrWhiteSpace(nameEditor.Text))
                {
                    await DisplayAlert("Ошибка!", "Заполните название сериала", "Оk");
                    return;
                }
                string searchImgName = string.Concat(nameEditor.Text);
                await SearchImages(searchImgName);
                return;
            }
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
        catch (Exception ex)
        {
            var ee = ex.Message;
        }
    }
}