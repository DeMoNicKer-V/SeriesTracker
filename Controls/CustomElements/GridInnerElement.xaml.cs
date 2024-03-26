using System.Windows.Input;

namespace SeriesTracker.Controls.CustomElements;

public partial class GridInnerElement : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(ElementBorder), string.Empty);

    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(ElementBorder), string.Empty);

    public static readonly BindableProperty ClickCommandProperty =
        BindableProperty.Create(nameof(ClickCommand), typeof(ICommand), typeof(ElementBorder), null);

    public static readonly BindableProperty IsBusyProperty =
        BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(ElementBorder), false);

    public GridInnerElement()
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

    public ICommand ClickCommand
    {
        get => (ICommand)GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        if (ClickCommand?.CanExecute(null) == true) ClickCommand?.Execute(null);
    }
}