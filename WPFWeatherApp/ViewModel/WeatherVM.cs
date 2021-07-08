using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WPFWeatherApp.Model;
using WPFWeatherApp.ViewModel.Commands;
using WPFWeatherApp.ViewModel.Helpers;

namespace WPFWeatherApp.ViewModel
{
    public class WeatherVM : INotifyPropertyChanged
    {
        private string query;

        public string Query
        {
            get { return query; }
            set 
            { 
                query = value;
                OnPropertyChanged("Query");
            }
        }

        private CurrentConditions currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return currentConditions; }
            set 
            { 
                currentConditions = value;
                OnPropertyChanged("CurrentConditions");
            }
        }

        private City selectedCity;

        public City SelectedCity
        {
            get { return selectedCity; }
            set 
            { 
                selectedCity = value;
                if (selectedCity != null)
                {
                    OnPropertyChanged("SelectedCity");
                    GetCurrentConditionsAsync();
                }
            }
        }

        public ObservableCollection<City> Cities { get; set; }

        public SearchCommand SearchCommand { get; set; }

        public WeatherVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                SelectedCity = new City
                {
                    LocalizedName = "New York"
                };

                CurrentConditions = new CurrentConditions
                {
                    WeatherText = "Partly Cloudy",
                    Temperature = new Temperature
                    {
                        Metric = new Units
                        {
                            Value = 21
                        }
                    }
                }; 
            }

            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
        }

        private async System.Threading.Tasks.Task GetCurrentConditionsAsync()
        {
            Query = string.Empty;
            CurrentConditions = await AccuWeatherHelper.GetCurrentConditionsAsync(SelectedCity.Key);
            Cities.Clear();
        }

        public async System.Threading.Tasks.Task MakeQueryAsync()
        {
            var cities = await AccuWeatherHelper.GetCitiesAsync(Query);

            Cities.Clear();
            foreach (var city in cities)
            {
                Cities.Add(city);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
