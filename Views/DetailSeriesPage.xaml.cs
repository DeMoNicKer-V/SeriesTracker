using AngleSharp.Common;
using CommunityToolkit.Maui.Behaviors;
using SeriesTracker.Models;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class DetailSeriesPage : ContentPage
{
    DetailSeriesPageViewModel detailSeriesPageView;
    public DetailSeriesPage()
    {
        InitializeComponent();
        this.BindingContext = detailSeriesPageView = new DetailSeriesPageViewModel();
    }

    public DetailSeriesPage(Series series)
    {
        InitializeComponent();
        this.BindingContext = detailSeriesPageView = new DetailSeriesPageViewModel();

        if (series != null)
        {
            detailSeriesPageView.Series = series;
            if (!series.isFavourite) { favoriteImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
            if (series.seriesRating < 0.5) { ratingImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
        }
    }

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
        OnAppearing();
        //ratingTintColor.TintColor = Colors.Yellow;
        // myLabel2.Text = mySlider.Value.ToString();
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
        editEpisodeEntry.IsVisible = false;
        editEpisodeEntry.Text = string.Empty;
        placeHolder.IsVisible = true;
        editEpisodeEntry.Unfocus();
        ShowBottomSheet();
    }

    private async void ShowBottomSheet()
    {
        // Показываем BottomSheet с анимацией
        await BottomSheet.TranslateTo(0, 300, 300);
        BottomSheet.IsVisible = true;

        // Скрываем BottomSheet с анимацией
        await BottomSheet.TranslateTo(0, 0, 300);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        detailSeriesPageView.OnAppearing();
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
        placeHolder.IsVisible = !placeHolder.IsVisible;
        if (editEpisodeEntry.IsVisible)
        {
            editEpisodeEntry.Focus();
        }
        else editEpisodeEntry.Unfocus();
    }

    private void episodeEntry_Completed(object sender, EventArgs e)
    {
        editEpisodeEntry.IsVisible = false;
        placeHolder.IsVisible = true;
        editEpisodeEntry.Text = string.Empty;
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        ratingExpander.IsExpanded = !ratingExpander.IsExpanded;
    }

    void ReloadPage() 
    {
        
            Behavior toRemove = favoriteImage.Behaviors.FirstOrDefault(b => b is IconTintColorBehavior);
            if (toRemove != null)
            {
                favoriteImage.Behaviors.Remove(toRemove);
            }
        
        else { favoriteImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
    }
    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        detailSeriesPageView.Series.isFavourite = !detailSeriesPageView.Series.isFavourite;
        ReloadPage();
    }
}