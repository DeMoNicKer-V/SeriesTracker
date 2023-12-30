using Microsoft.Maui;

namespace SeriesTracker.Controls.PopUp;

public partial class DetailPopUp : CommunityToolkit.Maui.Views.Popup
{
	public DetailPopUp()
	{
		InitializeComponent();
	}
    async void OnYesButtonClicked(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(myEntry.Text);
    }

    async void OnNoButtonClicked(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(myEntry.Text);
    }

    async void myEntry_Completed(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(myEntry.Text);
    }
}