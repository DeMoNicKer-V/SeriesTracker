using SeriesTracker.Models;

#if ANDROID

using static Android.Media.MediaRouter;

#endif

namespace SeriesTracker.ViewModels
{
    public partial class DetailSeriesPageViewModel : BaseSeriesModel
    {
        public Command CloseCommand { get; }

        public Command DeleteCommand { get; }

        public Command DetachCommand { get; }

        public Command EditCommand { get; }

        public Command BackCommand { get; }
        public DetailSeriesPageViewModel()
        {
            BackCommand = new Command(OnBackCommand);
            EditCommand = new Command(OnEditCommand);
            CloseCommand = new Command(OnCloseCommand);
            DeleteCommand = new Command(OnDeleteCommand);
            DetachCommand = new Command(OnDetachCommand);
            Series = new Series(); 
        }

        private async void OnCloseCommand()
        {
            // Скрываем BottomSheet с анимацией
            await Shell.Current.DisplayAlert("Закрыть", "aa", "aaa");
        }

        private async void OnDeleteCommand()
        {
            await Shell.Current.DisplayAlert("Удалить", "aa", "aaa");
        }

        private async void OnDetachCommand()
        {
            await Shell.Current.DisplayAlert("Открепить", "aa", "aaa");
        }

        private async void OnEditCommand()
        {
            await Shell.Current.DisplayAlert("Изменить", "aa", "aaa");
        }

        private async void OnBackCommand() 
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}