using SeriesTracker.Controls;
using CommunityToolkit.Maui.Views;

namespace SeriesTracker.Controls.BottomSheet;

public partial class BottomSheet : ContentView
{
    public static readonly BindableProperty TitleProperty =
                BindableProperty.Create(nameof(Title), typeof(string), typeof(BottomSheet), string.Empty);

    public static readonly BindableProperty myContePageProperty =
            BindableProperty.Create(nameof(ContentPageBehavior), typeof(ContentPage), typeof(BottomSheet));

    public static readonly BindableProperty ImageButtonProperty =
        BindableProperty.Create(nameof(ContentPageBehavior), typeof(ImageButton), typeof(BottomSheet));

    public static readonly BindableProperty CloseCommandProperty =
        BindableProperty.Create(nameof(CloseCommand), typeof(Command), typeof(BottomSheet));

    public static readonly BindableProperty EditCommandProperty =
        BindableProperty.Create(nameof(EditCommand), typeof(Command), typeof(BottomSheet));

    public static readonly BindableProperty DetachCommandProperty =
        BindableProperty.Create(nameof(DetachCommand), typeof(Command), typeof(BottomSheet));

    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(nameof(DeleteCommand), typeof(Command), typeof(BottomSheet));

    public static readonly BindableProperty DetachTextProperty =
    BindableProperty.Create(nameof(DetachText), typeof(string), typeof(BottomSheet), string.Empty);

    public BottomSheet()
    {
        InitializeComponent();
    }

    public ImageButton ImageButton
    {
        get => (ImageButton)GetValue(ImageButtonProperty);
        set => SetValue(ImageButtonProperty, value);
    }
    public ContentPage ContentPageBehavior
    {
        get => (ContentPage)GetValue(myContePageProperty);
        set => SetValue(myContePageProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string DetachText
    {
        get => (string)GetValue(DetachTextProperty);
        set => SetValue(DetachTextProperty, value);
    }

    public Command CloseCommand
    {
        get => (Command)GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    public Command EditCommand
    {
        get => (Command)GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }

    public Command DetachCommand
    {
        get => (Command)GetValue(DetachCommandProperty);
        set => SetValue(DetachCommandProperty, value);
    }

    public Command DeleteCommand
    {
        get => (Command)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        await ImageButton.RotateXTo(0, 200);
        await this.TranslateTo(0, 200, 100);
        this.IsVisible = false;
    }

    private void EditButton_Clicked(object sender, EventArgs e)
    {
        EditCommand?.Execute(null);
    }

    private void DetachButton_Clicked(object sender, EventArgs e)
    {
        DetachCommand?.Execute(null);
    }
    public async Task OpenBottomSheet()
    {
        await ImageButton.RotateXTo(180, 200);
        await this.TranslateTo(0, 200, 100);
        await this.TranslateTo(0, 0, 100);
        this.IsVisible = true;
    }
    public async Task CloseBottomSheet()
    {
        await ImageButton.RotateXTo(0, 200);
        await this.TranslateTo(0, 200, 100);
        this.IsVisible = false;
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        var popup = new SeriesTracker.Controls.DeleteAlert.DeleteAlert();
        popup.Title = Title;
        var result = await ContentPageBehavior.ShowPopupAsync(popup);

        if (result is bool boolResult)
            if (boolResult)
            {
                DeleteCommand?.Execute(null);
            }
            else
            {
                return;
            }
    }
}
