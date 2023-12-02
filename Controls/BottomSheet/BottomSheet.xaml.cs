namespace SeriesTracker.Controls.BottomSheet;

public partial class BottomSheet : ContentView
{
    public static readonly BindableProperty TitleProperty =
                BindableProperty.Create(nameof(Title), typeof(string), typeof(BottomSheet), string.Empty);

    public static readonly BindableProperty CloseCommandProperty =
        BindableProperty.Create(nameof(CloseCommand), typeof(Command), typeof(BottomSheet));

    public static readonly BindableProperty EditCommandProperty =
        BindableProperty.Create(nameof(EditCommand), typeof(Command), typeof(BottomSheet));

    public static readonly BindableProperty DetachCommandProperty =
        BindableProperty.Create(nameof(DetachCommand), typeof(Command), typeof(BottomSheet));

    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(nameof(DeleteCommand), typeof(Command), typeof(BottomSheet));

    public BottomSheet()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
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

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        CloseCommand?.Execute(null);
    }

    private void EditButton_Clicked(object sender, EventArgs e)
    {
        EditCommand?.Execute(null);
    }

    private void DetachButton_Clicked(object sender, EventArgs e)
    {
        DetachCommand?.Execute(null);
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        DeleteCommand?.Execute(null);
    }
}
