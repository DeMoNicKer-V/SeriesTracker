using SeriesTracker.Classes;
using SeriesTracker.Classes.Shikimori;
using SeriesTracker.ViewModels;

namespace SeriesTracker.Views;

public partial class AnimeDetailPage : ContentPage
{
    AnimeDetailPageViewModel animeDetailPageViewModel;
    public AnimeDetailPage()
	{
		InitializeComponent();
        this.BindingContext = animeDetailPageViewModel = new AnimeDetailPageViewModel(Navigation);
    }

    public AnimeDetailPage(AnimeBase anime)
    {
        InitializeComponent();
        this.BindingContext = animeDetailPageViewModel = new AnimeDetailPageViewModel(Navigation);

        if (anime != null)
        {
            animeDetailPageViewModel.Anime = anime;
        }
    }

    private async void DescriptionAreaChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {
        if (descriptionExpander.IsExpanded)
        {
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
}