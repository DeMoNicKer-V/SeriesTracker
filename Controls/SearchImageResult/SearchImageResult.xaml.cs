using CommunityToolkit.Maui.Views;
using SeriesTracker.Services.Exceptions;
using SeriesTracker.Services.GoogleApi;
using System.Xml.Linq;

namespace SeriesTracker.Controls.SearchImageResult;

public partial class SearchImageResult : Popup
{
    public static readonly BindableProperty SearchNextCommandProperty =
        BindableProperty.Create(nameof(SearchNextCommand), typeof(Command), typeof(SearchImageResult));

    public Command SearchNextCommand
    {
        get => (Command)GetValue(SearchNextCommandProperty);
        set => SetValue(SearchNextCommandProperty, value);
    }

    public static readonly BindableProperty FirstImageSourceProperty =
        BindableProperty.Create(nameof(FirstImageSource), typeof(string), typeof(SearchImageResult));

    public string FirstImageSource
    {
        get => (string)GetValue(FirstImageSourceProperty);
        set => SetValue(FirstImageSourceProperty, value);
    }
    public static readonly BindableProperty SecondImageSourceProperty =
    BindableProperty.Create(nameof(SecondImageSource), typeof(string), typeof(SearchImageResult));

    public string SecondImageSource
    {
        get => (string)GetValue(SecondImageSourceProperty);
        set => SetValue(SecondImageSourceProperty, value);
    }
    public static readonly BindableProperty ThirdImageSourceProperty =
    BindableProperty.Create(nameof(ThirdImageSource), typeof(string), typeof(SearchImageResult));

    public string ThirdImageSource
    {
        get => (string)GetValue(ThirdImageSourceProperty);
        set => SetValue(ThirdImageSourceProperty, value);
    }

    public enum ACTIVE_IMAGE
    {
        FIRST,
        SECOND,
        THIRD
    }

    public static readonly BindableProperty ActiveImageProperty = BindableProperty.Create(nameof(ActiveImage), typeof(int), typeof(SearchImageResult), 0);
    public int ActiveImage
    {
        get => (int)GetValue(ActiveImageProperty);
        set => SetValue(ActiveImageProperty, value);
    }

    public static readonly BindableProperty SearchNameProperty = BindableProperty.Create(nameof(ActiveImage), typeof(string), typeof(SearchImageResult), "");
    public string SearchName
    {
        get => (string)GetValue(SearchNameProperty);
        set => SetValue(SearchNameProperty, value);
    }

    public static readonly BindableProperty ImagesProperty = BindableProperty.Create(nameof(Images), typeof(IEnumerable<string>), typeof(SearchImageResult));
    public IEnumerable<string> Images
    {
        get => (IEnumerable<string>)GetValue(ImagesProperty);
        set => SetValue(ImagesProperty, value);
    }

    public static readonly BindableProperty PageProperty = BindableProperty.Create(nameof(Page), typeof(ContentPage), typeof(SearchImageResult));
    public ContentPage Page
    {
        get => (ContentPage)GetValue(PageProperty);
        set => SetValue(PageProperty, value);
    }

    public SearchImageResult()
	{
        ActiveImage = Convert.ToInt32(ACTIVE_IMAGE.FIRST);
        googleApiService = new();
        serchPageParam = 1;
        InitializeComponent();

    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        ActiveImage = Convert.ToInt32(ACTIVE_IMAGE.FIRST);
    }

    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        ActiveImage = Convert.ToInt32(ACTIVE_IMAGE.SECOND);
    }

    private void TapGestureRecognizer_Tapped_3(object sender, TappedEventArgs e)
    {
        ActiveImage = Convert.ToInt32(ACTIVE_IMAGE.THIRD);
    }

    private async void ConfirmBtn_Clicked(object sender, EventArgs e)
    {
        if (Images == null || Images.Any(s => s == null)) { await CloseAsync(false); return; }
        await CloseAsync(true);
    }
    private int serchPageParam;
    private readonly GoogleCustomSearchApiService googleApiService;
    private async Task GetImages() 
    {
        try
        {
            Images = await googleApiService.SearchAsync(SearchName, serchPageParam, 3);
            FirstImageSource = Images.ElementAt(0);
            SecondImageSource = Images.ElementAt(1);
            ThirdImageSource = Images.ElementAt(2);
            serchPageParam++;
        }
        catch (LimitQuotaCustomSeachException ex)
        {
            await CloseAsync(false);
            await Page.DisplayAlert("Превышен лимит поиска.", ex.Message, "Ок");
        }

    }

    private async void DenyBtn_Clicked(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }

    private async void NextSearch_Tapped(object sender, TappedEventArgs e)
    {
        await GetImages();
    }

    private async void this_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
       await GetImages();
    }
}