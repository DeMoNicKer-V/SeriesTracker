using CommunityToolkit.Maui.Views;

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

    public SearchImageResult()
	{
        ActiveImage = Convert.ToInt32(ACTIVE_IMAGE.FIRST);

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
        await CloseAsync(true);
    }

    private async void DenyBtn_Clicked(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }
}