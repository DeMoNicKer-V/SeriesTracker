using AngleSharp;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
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
            if (Convert.ToInt32(durationEntry.Text) > 0) { durationCheckBox.IsChecked = true; }
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

            string text = "����������� �������� �����������";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
            return false;
        }
        return true;
    }

    private void currentEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            currentEntry.HasError = true;
        }
        else currentEntry.HasError = false;
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
            descriptionCaption.Text = "������ ��������";
            await descriptionImage.RotateXTo(180, 200);
        }
        else
        {
            descriptionCaption.Text = "������� ��������";
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

    private void lastEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            lastEntry.HasError = true;
        }
        else lastEntry.HasError = false;
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

    private void startEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            startEntry.HasError = true;
        }
        else startEntry.HasError = false;
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            bool displayResult = await DisplayAlert("����� �����������", "������ ����������� � ���������?", "��", "���");
            if (displayResult == true)
            {
                if (string.IsNullOrWhiteSpace(nameEditor.Text))
                {
                    await DisplayAlert("������!", "��������� �������� �������", "�k");
                    return;
                }
                string name = nameEditor.Text.Replace(" ", "%20") + " ������";
                await Task.Run(async () => await Browser.Default.OpenAsync("https://yandex.by/images/search?text=" + name, BrowserLaunchMode.SystemPreferred));
            }
            PickOptions options = new()
            {
                PickerTitle = "����������, �������� ����������� ��� �������!",
                FileTypes = FilePickerFileType.Images,
            };
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                posterImage.Source = result.FullPath;
                Behavior toRemove = posterImage.Behaviors.FirstOrDefault(b => b is IconTintColorBehavior);
                if (toRemove != null)
                {
                    posterImage.Behaviors.Remove(toRemove);
                }
                ((NewSeriesPageViewModel)BindingContext).Series.imagePath = result.FullPath;
                ChangePosterAttributes();
            }
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }
    }
}