using CommunityToolkit.Maui.Views;

namespace SeriesTracker.Controls.CustomPopUp;

public partial class CustomAlert : Popup
{
	public CustomAlert()
	{
		InitializeComponent();
	}
    public CustomAlert(string title, string text, string denytext, string confirmtext)
    {
        InitializeComponent();
        Title = title;
        Text = text;
        DenyText = denytext;
        ConfirmButton = true;
        ConfirmText = confirmtext;
    }
    public CustomAlert(string title, string text, string denytext)
    {
        InitializeComponent();
        Title = title;
        Text = text;
        DenyText = denytext;
    }

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomAlert), string.Empty);
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty TextProperty =
    BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomAlert), string.Empty);
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty ConfirmTextProperty =
BindableProperty.Create(nameof(ConfirmText), typeof(string), typeof(CustomAlert), string.Empty);
    public string ConfirmText
    {
        get => (string)GetValue(ConfirmTextProperty);
        set => SetValue(ConfirmTextProperty, value);
    }

    public static readonly BindableProperty DenyTextProperty =
BindableProperty.Create(nameof(DenyText), typeof(string), typeof(CustomAlert), string.Empty);
    public string DenyText
    {
        get => (string)GetValue(DenyTextProperty);
        set => SetValue(DenyTextProperty, value);
    }

    public static readonly BindableProperty ConfirmButtonProperty =
BindableProperty.Create(nameof(ConfirmButton), typeof(bool), typeof(CustomAlert), false);
    public bool ConfirmButton
    {
        get => (bool)GetValue(ConfirmButtonProperty);
        set => SetValue(ConfirmButtonProperty, value);
    }

    private async void deny_Clicked(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }
    private async void confirm_Clicked(object sender, EventArgs e)
    {
        await CloseAsync(true);
    }

}