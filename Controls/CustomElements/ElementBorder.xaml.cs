using System.Windows.Input;

namespace SeriesTracker.Controls;

public partial class ElementBorder : ContentView
{
    public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ElementBorder), string.Empty);

    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(ElementBorder), string.Empty);

    public static readonly BindableProperty TappedCommandProperty =
        BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(ElementBorder));

    public static readonly BindableProperty TextColorProperty =
    BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ElementBorder));

    public ElementBorder()
	{
		InitializeComponent();
	}

    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ICommand TappedCommand
    {
        get => (ICommand)GetValue(TappedCommandProperty);
        set => SetValue(TappedCommandProperty, value);
    }
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    private void TextLabelTapped(object sender, TappedEventArgs e)
    {
        TappedCommand?.Execute(null);
    }
}