namespace SeriesTracker.Controls.MaterialEntry;

public partial class MaterialEntry : ContentView
{
    private readonly int _yScale;
    private readonly int _xScale;

    private readonly Color _primary;

    public MaterialEntry()
    {
        InitializeComponent();

        var rd = App.Current.Resources.MergedDictionaries.First();

        _primary = (Color)rd["Primary"];

        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            _yScale = 27;
            _xScale = 0;
        }
        else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            _yScale = -15;
            _xScale = -40;
        }
        else if (DeviceInfo.Current.Platform == DevicePlatform.iOS || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
            _yScale = -22;
            _xScale = -50;
        }

        MEEntry.ZIndex = 2;
        MEBorder.ZIndex = 2;
        MELabel.ZIndex = 3;
        warning.ZIndex = 3;
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(MaterialEntry), null, BindingMode.TwoWay);
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty LabelProperty = BindableProperty.Create(nameof(Label), typeof(string), typeof(MaterialEntry), null);
    public string Label
    {
        get => (String)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly BindableProperty HasErrorProperty = BindableProperty.Create(nameof(HasError), typeof(bool), typeof(MaterialEntry), null, BindingMode.TwoWay);
    public bool HasError
    {
        get => (bool)GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }
    public event EventHandler<TextChangedEventArgs> TextChanged;
    protected virtual void OnTextChanged(TextChangedEventArgs e)
    {
        EventHandler<TextChangedEventArgs> handler = TextChanged;
        handler?.Invoke(this, e);
    }
    public event EventHandler<EventArgs> Completed;
    protected virtual void OnCompleted(EventArgs e)
    {
        EventHandler<EventArgs> handler = Completed;
        handler?.Invoke(this, e);
    }
    private void MEEntry_Focused(object sender, FocusEventArgs e)
    {
        //MELabel.IsVisible = false;
        MEBorder.Stroke = (Color)_primary;
        MELabel.TextColor = (Color)_primary;
        ScaleLabelDown();
    }

    private void MEEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MEEntry.Text))
        {
            warning.TranslateTo(-50, -18, 250, Easing.Linear);
            ScaleLabelUp();
        }
        else
        {
            if (DeviceInfo.Current.Platform == DevicePlatform.iOS || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
            {
                MELabel.TranslateTo(_xScale, _yScale + 2, 50, Easing.Default);
            }
        }
        MEEntry.IsEnabled = false;
        MEEntry.IsEnabled = true;
    }

    private void ScaleLabelDown()
    {
        MELabel.ScaleTo(0.85, 250, Easing.Linear);
        MELabel.TranslateTo(_xScale, _yScale, 250, Easing.Linear);
        MELabel.ZIndex = 3;
        MEEntry.WidthRequest = 50;
        warning.TranslateTo(-35, -18, 250, Easing.Linear);
    }

    private void ScaleLabelUp()
    {
        MELabel.ZIndex = 1;
        MELabel.ScaleTo(1, 250, Easing.Linear);
        MELabel.TranslateTo(0, 0, 250, Easing.Linear);
        MEEntry.WidthRequest = 90;
    }

    private void MEEntry_Loaded(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(MEEntry.Text))
        {
            ScaleLabelDown();
        }
        }

    private void MEEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        OnTextChanged(e);
    }

    private void MEEntry_Completed(object sender, EventArgs e)
    {
        OnCompleted(e);
    }
}