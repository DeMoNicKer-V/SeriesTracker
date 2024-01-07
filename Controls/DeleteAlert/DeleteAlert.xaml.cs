using CommunityToolkit.Maui.Views;
using static Android.Icu.Text.CaseMap;

namespace SeriesTracker.Controls.DeleteAlert;

public partial class DeleteAlert : Popup
{
    public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(DeleteAlert), string.Empty);
    public DeleteAlert()
	{
		InitializeComponent();
	}
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
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