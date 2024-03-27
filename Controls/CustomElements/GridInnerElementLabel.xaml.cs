using static System.Net.Mime.MediaTypeNames;

namespace SeriesTracker.Controls.CustomElements;

public partial class GridInnerElementLabel : ContentView
{
    public static readonly BindableProperty MainTextProperty =
        BindableProperty.Create(nameof(MainText), typeof(string), typeof(GridInnerElementLabel), string.Empty);

    public static readonly BindableProperty SubTextProperty =
        BindableProperty.Create(nameof(SubText), typeof(string), typeof(GridInnerElementLabel), string.Empty);

    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(GridInnerElementLabel), string.Empty);
    public GridInnerElementLabel()
	{
		InitializeComponent();
	}
    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public string MainText
    {
        get => (string)GetValue(MainTextProperty);
        set => SetValue(MainTextProperty, value);
    }

    public string SubText
    {
        get => (string)GetValue(SubTextProperty);
        set => SetValue(SubTextProperty, value);
    }
}