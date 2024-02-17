using CommunityToolkit.Maui.Behaviors;

namespace SeriesTracker.Controls;

public partial class RatingSlider : ContentView
{
	public RatingSlider()
	{
		InitializeComponent();
	}

    private const double Step = 0.5;
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(RatingSlider), null, BindingMode.TwoWay);
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    public static readonly BindableProperty OldValueProperty = BindableProperty.Create(nameof(OldValue), typeof(double), typeof(RatingSlider), null, BindingMode.TwoWay);
    public double OldValue
    {
        get => (double)GetValue(OldValueProperty);
        set => SetValue(OldValueProperty, value);
    }

    public static readonly BindableProperty RatingPictureProperty = BindableProperty.Create(nameof(RatingPicture), typeof(Image), typeof(RatingSlider), null, BindingMode.TwoWay);
    public Image RatingPicture
    {
        get => (Image)GetValue(RatingPictureProperty);
        set => SetValue(RatingPictureProperty, value);
    }

    public static readonly BindableProperty RatingLabelProperty = BindableProperty.Create(nameof(RatingLabel), typeof(Label), typeof(RatingSlider), null, BindingMode.TwoWay);
    public Label RatingLabel
    {
        get => (Label)GetValue(RatingLabelProperty);
        set => SetValue(RatingLabelProperty, value);
    }

    public static readonly BindableProperty ExpanderBehaviorProperty = BindableProperty.Create(nameof(ExpanderBehavior), typeof(bool), typeof(RatingSlider), null, BindingMode.TwoWay);
    public bool ExpanderBehavior
    {
        get => (bool)GetValue(ExpanderBehaviorProperty);
        set => SetValue(ExpanderBehaviorProperty, value);
    }

    public static readonly BindableProperty CancelCommandProperty =
    BindableProperty.Create(nameof(CancelCommand), typeof(Command), typeof(RatingSlider));

    public Command CancelCommand
    {
        get => (Command)GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public static readonly BindableProperty AcceptCommandProperty =
        BindableProperty.Create(nameof(AcceptCommand), typeof(Command), typeof(RatingSlider));

    public Command AcceptCommand
    {
        get => (Command)GetValue(AcceptCommandProperty);
        set => SetValue(AcceptCommandProperty, value);
    }

    private void AcceptButton_Clicked(object sender, EventArgs e)
    {
        AcceptCommand?.Execute(null);
        Behavior toRemove = RatingPicture.Behaviors.FirstOrDefault(b => b is IconTintColorBehavior);
        if (Value > 0.5)
        {
            RatingPicture.Behaviors.Remove(toRemove);
            RatingLabel.IsVisible = true;
        }

        else
        {
            RatingLabel.IsVisible = false;
            RatingPicture.Behaviors.Add(new IconTintColorBehavior { TintColor = Microsoft.Maui.Graphics.Color.FromArgb("#ACACAC") });
        }
        RatingLabel.Text = BaseSlider.Value.ToString();
        ExpanderBehavior = false;
    }

    private void RatingSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Slider slider = (Slider)sender;
        slider.Value = Math.Round(e.NewValue / Step) * Step;
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        BaseSlider.Value = OldValue;
        CancelCommand?.Execute(null);
        ExpanderBehavior = false;
    }
}