namespace SeriesTracker.Controls;

public partial class CustomEmptyView : ContentView
{
    public static readonly BindableProperty MainTextProperty = BindableProperty.Create(nameof(MainText), typeof(string), typeof(CustomEmptyView), string.Empty, BindingMode.OneTime);
    public string MainText
    {
        get => (string)GetValue(MainTextProperty);
        set => SetValue(MainTextProperty, value);
    }

    public static readonly BindableProperty SubTextProperty = BindableProperty.Create(nameof(SubText), typeof(string), typeof(CustomEmptyView), string.Empty, BindingMode.OneTime);
    public string SubText
    {
        get => (string)GetValue(SubTextProperty);
        set => SetValue(SubTextProperty, value);
    }

    public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(CustomEmptyView), false, BindingMode.OneTime);
    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }
    public CustomEmptyView()
	{
		InitializeComponent();
	}
}