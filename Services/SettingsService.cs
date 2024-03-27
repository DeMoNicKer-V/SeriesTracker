using CommunityToolkit.Mvvm.ComponentModel;
using SeriesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services
{
    public partial class SettingsService : ObservableObject
    {
        private static SettingsService _instance;
        public static SettingsService Instance => _instance ??= new SettingsService();

        private SettingsService()
        {
            Theme = SelectTheme(Preferences.Get("AppTheme", "Unspecified"));
            Sync = SelectSync(Preferences.Get("SyncType", 0));
        }

        private Theme SelectTheme(string preferenceValue)
        {
            switch (preferenceValue)
            {
                case "Unspecified": return Theme.System;
                case "Dark": return Theme.Dark;
                case "Light": return Theme.Light;
                default: return Theme.System;
            }
        }

        private Sync SelectSync(int syncId)
        {
            switch (syncId)
            {
                case 0: return Sync.Off;
                case 1: return Sync.OneDay;
                case 2: return Sync.OneWeek;
                default: return Sync.Off;
            }
        }


        [ObservableProperty]
        private Theme _theme;
        [ObservableProperty]
        private Sync _sync;
    }

}
