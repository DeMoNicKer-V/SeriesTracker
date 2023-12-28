using SeriesTracker.Models;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class DetailSeriesPage : ContentPage
{
    public DetailSeriesPage()
    {
        InitializeComponent();
        this.BindingContext = new DetailSeriesPageViewModel();
    }

    public DetailSeriesPage(Series series)
    {
        InitializeComponent();
        this.BindingContext = new DetailSeriesPageViewModel();

        if (series != null)
        {
            ((DetailSeriesPageViewModel)BindingContext).Series = series;
            /*var seriesProgress = ((double)series.currentEpisode / (double)series.lastEpisode) * 100.0;
            episodeProgress.Progress = (float)Math.Round(seriesProgress);*/
        }
    }

   /* public Command CloseCommand { get; }

    public Command DeleteCommand { get; }

    public Command DetachCommand { get; }

    public Command EditCommand { get; }*/

    public Series Series
    {
        get; set;
    }
    private void cancelButton_Clicked(object sender, EventArgs e)
    {
        ratingExpander.IsExpanded = false;
        mySlider.Value = 0;
    }

    private async void Expander_ExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {
        if (descriptionExpander.IsExpanded)
        {
            OnCloseCommand();
            descriptionCaption.Text = "Скрыть описание";
            await descriptionImage.RotateXTo(180, 200);
            await baseContainer.ScrollToAsync(gridContainer, ScrollToPosition.End, true);
        }
        else
        {
            descriptionCaption.Text = "Открыть описание";
            await descriptionImage.RotateXTo(0, 200);
        }
    }

    private void likeButton_Clicked(object sender, EventArgs e)
    {
        ratingExpander.IsExpanded = false;
        ratingTintColor.TintColor = Colors.Yellow;
        myLabel2.Text = mySlider.Value.ToString();
    }

    private async void OnCloseCommand()
    {
        // Скрываем BottomSheet с анимацией
        await BottomSheet.TranslateTo(0, 300, 300);
        BottomSheet.IsVisible = false;
    }
/*
    private async void OnDeleteCommand()
    {
        await DisplayAlert("Удалить", "aa", "aaa");
    }

    private async void OnDetachCommand()
    {
        await DisplayAlert("Открепить", "aa", "aaa");
    }

    private async void OnEditCommand()
    {
        await DisplayAlert("Изменить", "aa", "aaa");
    }
   */
    private void OpenButton_Clicked(object sender, EventArgs e)
    {
        descriptionExpander.IsExpanded = false;
        ShowBottomSheet();
    }

    private void ratingButton_Clicked(object sender, EventArgs e)
    {
        ratingExpander.IsExpanded = !ratingExpander.IsExpanded;
    }

    private async void ShowBottomSheet()
    {
        // Показываем BottomSheet с анимацией
        await BottomSheet.TranslateTo(0, 300, 300);
        BottomSheet.IsVisible = true;

        // Скрываем BottomSheet с анимацией
        await BottomSheet.TranslateTo(0, 0, 300);
    }

    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 0.5;
        Slider slider = (Slider)sender;
        slider.Value = Math.Round(e.NewValue / step) * step;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        editEpisodeEntry.IsVisible = !editEpisodeEntry.IsVisible;
        if (editEpisodeEntry.IsVisible)
        {
            editEpisodeEntry.Focus();
        }
    }

    private void episodeEntry_Completed(object sender, EventArgs e)
    {
        editEpisodeEntry.IsVisible = false;
        episodeEntry.Text = string.Empty;
    }
}