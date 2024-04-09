using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SeriesTracker.Views;
using SeriesTracker.Services;
using System.Windows.Input;
using static SeriesTracker.Services.Constant.SeriesBaseParameters;

namespace SeriesTracker;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public ICommand CenterViewCommand { get; } = new Command(async () => await Shell.Current.GoToAsync(nameof(NewSeriesPage)));

    [RelayCommand]
    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        if (args.Source == ShellNavigationSource.PopToRoot)
        {
            WeakReferenceMessenger.Default.Send(new TabbarChangedMessage(true));
        }
    }

    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);

        if (args.Source == ShellNavigationSource.Push)
        {
            WeakReferenceMessenger.Default.Send(new TabbarChangedMessage(false));
        }
        SkipItem = 0;
        if (args.Target.Location.OriginalString.Contains("MainPage"))
        {
            WachedFlag = false;
        }
        else if (args.Target.Location.OriginalString.Contains("SecondPage"))
        {
            WachedFlag = true;
        }
    }

    private async Task Sync(string date, int daysCount) 
    {
        if (string.IsNullOrWhiteSpace(date)) return;
        var lastDate = DateTime.Parse(date);
        if (lastDate.AddDays(daysCount) < DateTime.Now)
        {
            await ShowToast("Идет синхронизация");
            await App.FirebaseService.InSynchronize();
            await App.FirebaseService.OutSynchronize();
            await ShowToast("Синхронизация завершена");
        }
    }
    private async void Shell_Loaded(object sender, EventArgs e)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet) return;
        var SyncType = Preferences.Get("SyncType", 0);
        switch (SyncType)
        {
            case 0: return; 
            case 1: await Sync(Preferences.Get("LastSyncDate", ""),1); return;
            case 2: await Sync(Preferences.Get("LastSyncDate", ""),7); return;
            default:
                return;
        }
    }
}