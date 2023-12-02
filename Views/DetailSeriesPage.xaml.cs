using CommunityToolkit.Mvvm.Input;
using SeriesTracker.ViewModels;
using System.Windows.Input;

namespace SeriesTracker.Views;

public partial class DetailSeriesPage : ContentPage
{
    public Command EditCommand { get; }
    public Command CloseCommand { get; }
    public Command DeleteCommand { get; }
    public Command DetachCommand { get; }
    public DetailSeriesPage()
	{
		InitializeComponent();
        EditCommand = new Command(OnEditCommand);
        CloseCommand = new Command(OnCloseCommand);
        DeleteCommand = new Command(OnDeleteCommand);
        DetachCommand = new Command(OnDetachCommand);
        BindingContext = this;

    }
    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 0.5;
        Slider slider = (Slider)sender;
        slider.Value = Math.Round(e.NewValue / step) * step;
    }

    private void ratingButton_Clicked(object sender, EventArgs e)
    {
        ratingExpander.IsExpanded = !ratingExpander.IsExpanded;
    }

    private void likeButton_Clicked(object sender, EventArgs e)
    {
        ratingExpander.IsExpanded = false;
        ratingTintColor.TintColor = Colors.Yellow;
        myLabel2.Text = mySlider.Value.ToString();
    }

    private void cancelButton_Clicked(object sender, EventArgs e)
    {
        ratingExpander.IsExpanded = false;
        mySlider.Value = 0;
    }

 private async void ShowBottomSheet()
        {
            // Показываем BottomSheet с анимацией
            await BottomSheet.TranslateTo(0, 300, 300);
            BottomSheet.IsVisible = true;

            // Скрываем BottomSheet с анимацией
            await BottomSheet.TranslateTo(0, 0, 300);
        }


        private void OpenButton_Clicked(object sender, EventArgs e)
        {
            ShowBottomSheet();
        }

    private async void OnDetachCommand()
    {
        await DisplayAlert("Открепить", "aa", "aaa");
    }
    private async void OnEditCommand()
    {
        await DisplayAlert("Изменить", "aa", "aaa");
    }

    private async void OnCloseCommand()
    {
        // Скрываем BottomSheet с анимацией
        await BottomSheet.TranslateTo(0, 300, 300);
        BottomSheet.IsVisible = false;
    }

    private async void OnDeleteCommand()
    {
        await DisplayAlert("Удалить", "aa", "aaa");
    }

}
