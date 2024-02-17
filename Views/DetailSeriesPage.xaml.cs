using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core.Platform;
using SeriesTracker.Models;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class DetailSeriesPage : ContentPage
{
    private DetailSeriesPageViewModel detailSeriesPageView;

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
            BottomSheet.DetachText = stateString(series.isOver);
            detailSeriesPageView.Series = series;

            if (!series.isFavourite) { favoriteImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
            if (series.seriesRating < 0.5) { ratingLabel.IsVisible = false; ratingImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        detailSeriesPageView.OnAppearing();
    }

    private async void editEpisodeEntry_Unfocused(object sender, FocusEventArgs e)
    {
        await editEpisodeEntry.HideKeyboardAsync(CancellationToken.None);
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

    private async void OnCloseCommand()
    {
        await BottomSheet.CloseBottomSheet();
        BottomSheet.IsVisible = false;
    }

    private void OpenButton_Clicked(object sender, EventArgs e)
    {
        descriptionExpander.IsExpanded = false;
        ratingExpander.IsExpanded = false;
        editEpisodeEntry.IsVisible = false;
        editEpisodeEntry.Text = placeHolder.Text;
        placeHolder.IsVisible = true;
        editEpisodeEntry.Unfocus();
        if (!BottomSheet.shortDuration)
        {
            menuLabel.RotateXTo(180, 200);
            ShowBottomSheet();
        }
        else { menuLabel.RotateXTo(0, 200); OnCloseCommand(); }
    }

    private void ReloadPage()
    {
        Behavior toRemove = favoriteImage.Behaviors.FirstOrDefault(b => b is IconTintColorBehavior);
        if (toRemove != null)
        {
            favoriteImage.Behaviors.Remove(toRemove);
        }
        else { favoriteImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#ACACAC") }); }
    }

    private async void ShowBottomSheet()
    {
        await BottomSheet.OpenBottomSheet();
        BottomSheet.IsVisible = true;
    }

    private string stateString(bool isOver)
    {
        if (isOver)
        {
            return String.Format("Пометить как непросмотренное");
        }
        return String.Format("Пометить как просмотренное");
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
    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        ratingExpander.IsExpanded = !ratingExpander.IsExpanded;
        OnCloseCommand();
    }
    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        ReloadPage();
    }
}