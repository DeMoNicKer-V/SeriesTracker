using AngleSharp.Common;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core.Platform;
using SeriesTracker.Models;
using SeriesTracker.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace SeriesTracker.Views;

public partial class DetailSeriesPage : ContentPage
{
    DetailSeriesPageViewModel detailSeriesPageView;
    public DetailSeriesPage()
    {
        InitializeComponent();
        this.BindingContext = detailSeriesPageView = new DetailSeriesPageViewModel(Navigation);
        BottomSheet.ContentPageBehavior = this;
    }

    private string stateString(bool isOver)
    {
        if (isOver)
        {
            return String.Format("Пометить как непросмотренное");
        }
        return String.Format("Пометить как просмотренное");
    }
    public DetailSeriesPage(Series series)
    {
        InitializeComponent();
        this.BindingContext = detailSeriesPageView = new DetailSeriesPageViewModel(Navigation);
        BottomSheet.ContentPageBehavior = this;
        if (series != null)
        {
            BottomSheet.Title = series.seriesName;
            BottomSheet.DetachText = stateString(series.isOver);
            detailSeriesPageView.Series = series;
            if (!series.isFavourite) { favoriteImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
            if (series.seriesRating < 0.5) { myLabel2.IsVisible = false; ratingImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
        }
    }

    public Series Series
    {
        get; set;
    }
    private void cancelButton_Clicked(object sender, EventArgs e)
    {
        ratingExpander.IsExpanded = false;
        mySlider.Value = Convert.ToDouble(myLabel2.Text);
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
        ReloadPage2();
        myLabel2.Text = mySlider.Value.ToString();
        //ratingTintColor.TintColor = Colors.Yellow;
        // myLabel2.Text = mySlider.Value.ToString();
    }

    private async void OnCloseCommand()
    {
        // Скрываем BottomSheet с анимацией
        await BottomSheet.TranslateTo(0, 300, 200);
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
        ratingExpander.IsExpanded = false;
        editEpisodeEntry.IsVisible = false;
        editEpisodeEntry.Text = placeHolder.Text;
        placeHolder.IsVisible = true;
        editEpisodeEntry.Unfocus();
        if (!BottomSheet.IsVisible)
        {
            ShowBottomSheet();
        }
        else { OnCloseCommand(); }
    }

    private async void ShowBottomSheet()
    {
        // Показываем BottomSheet с анимацией
        await BottomSheet.TranslateTo(0, 200, 200);
        BottomSheet.IsVisible = true;
        if (labelSeriesName.Text.Length < 20)
        {
            await BottomSheet.TranslateTo(0, 25, 200);
            return;
        }
            
        // Скрываем BottomSheet с анимацией
        await BottomSheet.TranslateTo(0, 0, 200);
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
            editEpisodeEntry.CursorPosition = editEpisodeEntry.Text.Length;
        }
        else editEpisodeEntry.Unfocus();
    }

    private void episodeEntry_Completed(object sender, EventArgs e)
    {
        editEpisodeEntry.IsVisible = false;
        placeHolder.IsVisible = true;
        if (Convert.ToInt32(editEpisodeEntry.Text) > Convert.ToInt32(lastEpisodeEntry.Text) || Convert.ToInt32(editEpisodeEntry.Text) < 1)
        {
            editEpisodeEntry.Text = placeHolder.Text;
            return;
        }
        placeHolder.Text = editEpisodeEntry.Text;
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        ratingExpander.IsExpanded = !ratingExpander.IsExpanded;
        OnCloseCommand();
    }

    void ReloadPage2()
    {

        Behavior toRemove = ratingImage.Behaviors.FirstOrDefault(b => b is IconTintColorBehavior);
        if (mySlider.Value > 0.5)
        {
            ratingImage.Behaviors.Remove(toRemove);
            myLabel2.IsVisible = true;
        }

        else { myLabel2.IsVisible = false; ratingImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
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
        ReloadPage();
    }

    private async void editEpisodeEntry_Unfocused(object sender, FocusEventArgs e)
    {
        await editEpisodeEntry.HideKeyboardAsync(CancellationToken.None);
    }
}