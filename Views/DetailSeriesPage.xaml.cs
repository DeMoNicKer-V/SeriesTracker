using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core.Platform;
using SeriesTracker.Models;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class DetailSeriesPage : ContentPage
{
    private DetailSeriesPageViewModel detailSeriesPageView;

    private int rotate = 0;
    private int MenuImageRotation
    {
        get => rotate; set
        {
            if (rotate == 180)
            {
                rotate = 0;
            }
            else rotate = value;
        }
    }
    public DetailSeriesPage()
    {
        InitializeComponent();
        this.BindingContext = detailSeriesPageView = new DetailSeriesPageViewModel(Navigation);
        BottomSheet.ContentPageBehavior = this;
    }

    public DetailSeriesPage(Series series)
    {
        InitializeComponent();
        this.BindingContext = detailSeriesPageView = new DetailSeriesPageViewModel(Navigation);
        BottomSheet.ContentPageBehavior = this;
        if (series != null)
        {
            BottomSheet.Title = series.seriesName;
            BottomSheet.DetachText = GetStateString(series.isOver);
            detailSeriesPageView.Series = series;

            if (!series.isFavourite) { favoriteImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
            if (series.seriesRating < 1) { ratingLabel.IsVisible = false; ratingImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        detailSeriesPageView.OnAppearing();
    }

    private async void editEpisodeEntry_Unfocused(object sender, FocusEventArgs e)
    {
        editEpisodeEntry.IsVisible = false;
        editEpisodeEntry.Text = placeHolder.Text;
        placeHolder.IsVisible = true;
        await editEpisodeEntry.HideKeyboardAsync(CancellationToken.None);
    }

    private void EpisodeEntry_Completed(object sender, EventArgs e)
    {
        editEpisodeEntry.IsVisible = false;
        placeHolder.IsVisible = true;
        if (Convert.ToInt32(editEpisodeEntry.Text) > Convert.ToInt32(lastEpisodeEntry.Text) || Convert.ToInt32(editEpisodeEntry.Text) < 0)
        {
            editEpisodeEntry.Text = placeHolder.Text;
            return;
        }
        placeHolder.Text = editEpisodeEntry.Text;
    }

    private async void DescriptionAreaChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
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

    private async void OnCloseCommand()
    {
        await BottomSheet.CloseBottomSheet();
        BottomSheet.IsVisible = false;
    }
    private void ShowBottomSheet_Clicked(object sender, EventArgs e)
    {
        descriptionExpander.IsExpanded = false;
        ratingExpander.IsExpanded = false;
        editEpisodeEntry.Unfocus();
        MenuImageRotation = 180;
        menuButton.RotateXTo(MenuImageRotation, 200);

        if (!BottomSheet.IsVisible) { ShowBottomSheet();}
        else { OnCloseCommand(); }
    }

    private async void ShowBottomSheet()
    {
        await BottomSheet.OpenBottomSheet();
        BottomSheet.IsVisible = true;
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

    private void EditWatchedEpisode_Tapped(object sender, TappedEventArgs e)
    {
        ratingExpander.IsExpanded = !ratingExpander.IsExpanded;
        OnCloseCommand();
    }

    private void ReloadPage_Tapped(object sender, TappedEventArgs e)
    {
        Behavior toRemove = favoriteImage.Behaviors.FirstOrDefault(b => b is IconTintColorBehavior);
        if (toRemove != null)
        {
            favoriteImage.Behaviors.Remove(toRemove);
        }
        else { favoriteImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
    }

    private string GetStateString(bool isOver)
    {
        if (isOver)
        {
            return String.Format("Пометить как непросмотренное");
        }
        return String.Format("Пометить как просмотренное");
    }
}