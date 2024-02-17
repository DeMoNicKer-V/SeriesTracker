using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SeriesTracker.Services.Constant;
using SeriesTracker.ViewModels;
using SeriesTracker.Views;
using System.Windows.Input;

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
        if (args.Target != null)
        {
            if (args.Target.Location.OriginalString.Contains("MainPage"))
            {
                Parameters.WachedFlag = false;
            }
            else if (args.Target.Location.OriginalString.Contains("SecondPage"))
            {
                Parameters.WachedFlag = true;
            }    
        }
    }
}
