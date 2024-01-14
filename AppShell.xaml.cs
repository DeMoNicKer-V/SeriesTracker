using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace SeriesTracker;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        BindingContext = this;
    }
    [RelayCommand]
    public async void CenterViewCommand()
    {
        await Toast.Make("CenterViewCommand invoked!").Show();
    }
    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);

        // Cancel any back navigation.
        if (args.Source == ShellNavigationSource.Push)
        {
            WeakReferenceMessenger.Default.Send(new TabbarChangedMessage(false));
        }
      else if (args.Source == ShellNavigationSource.PopToRoot)
        {
            WeakReferenceMessenger.Default.Send(new TabbarChangedMessage(true));
        }
        // }
    }
}
