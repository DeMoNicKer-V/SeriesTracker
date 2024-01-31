using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SeriesTracker.Classes.Shikimori;
using SeriesTracker.Services.ShikimoriBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.ViewModels
{
    public partial class SeriesListPageViewModel : BaseSeriesModel
    {
        private ShikimoriBase ShikimoriBase;
        public SeriesListPageViewModel(INavigation navigation)
        { 
            Navigation = navigation; 
            ShikimoriBase = new ShikimoriBase(); 
        }

        public ObservableCollection<Anime> SeriesList
        {
            get
            {
                return seriesList;
            }
            set
            {
                seriesList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Anime> seriesList = new ObservableCollection<Anime>();
        [RelayCommand]
        private async Task LoadSeries()
        {
            IsBusy = true;
            try
            {
                var graphQLResponse = await ShikimoriBase.GetAnimesByName(1, "bakemono");
               var shikimoriAnimes = graphQLResponse.Data.Animes;
                if (shikimoriAnimes != null && shikimoriAnimes.Count() > 0)
                {
                    foreach (var item in shikimoriAnimes)
                    {
                        SeriesList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            SeriesList.Clear();
            IsBusy = true;
        }
    }


}
