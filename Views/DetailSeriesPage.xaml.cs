using AngleSharp.Common;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core.Platform;
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
            descriptionCaption.Text = "������ ��������";
            await descriptionImage.RotateXTo(180, 200);
            await baseContainer.ScrollToAsync(gridContainer, ScrollToPosition.End, true);
        }
        else
        {
            descriptionCaption.Text = "������� ��������";
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
        // �������� BottomSheet � ���������
        await BottomSheet.TranslateTo(0, 300, 200);
        BottomSheet.IsVisible = false;
    }
/*
    private async void OnDeleteCommand()
    {
        await DisplayAlert("�������", "aa", "aaa");
    }

    private async void OnDetachCommand()
    {
        await DisplayAlert("���������", "aa", "aaa");
    }

    private async void OnEditCommand()
    {
        await DisplayAlert("��������", "aa", "aaa");
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
        // ���������� BottomSheet � ���������
        await BottomSheet.TranslateTo(0, 200, 200);
        BottomSheet.IsVisible = true;
        if (labelSeriesName.Text.Length < 20)
        {
            await BottomSheet.TranslateTo(0, 25, 200);
            return;
        }
            
        // �������� BottomSheet � ���������
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
        editEpisodeEntry.Text = placeHolder.Text;
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
        detailSeriesPageView.Series.isFavourite = !detailSeriesPageView.Series.isFavourite;
        ReloadPage();
    }

    private async void editEpisodeEntry_Unfocused(object sender, FocusEventArgs e)
    {
        await editEpisodeEntry.HideKeyboardAsync(CancellationToken.None);
    }
}