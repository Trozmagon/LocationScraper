using Core.Models;
using GoogleMaps.LocationServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ILocationService = Core.Services.ILocationService;

namespace Bloop.LocationScraper
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly ILocationService _locationService;
        private readonly GoogleLocationService _googleLocationService;
        private List<Country> _countries;
        private List<State> _states;
        private List<City> _cities;

        public Startup(IConfiguration config, ILocationService locationService)
        {
            _config = config;
            _locationService = locationService;
            _googleLocationService = new GoogleLocationService(apikey: _config.GetSection("GoogleMapsAPIKey").Value);
        }

        public void Run()
        {
            PopulateWorld();

            var countries = GetCountries();

            foreach (var country in countries)
            {
                _locationService.CreateOrUpdateCountry(country);
            }
        }

        private void PopulateWorld()
        {
            var countriesJson = "";
            var statesJson = "";
            var citiesJson = "";

            using (var client = new WebClient())
            {
                try
                {
                    countriesJson = client.DownloadString($"{_config.GetSection("CountriesList").Value}");
                    statesJson = client.DownloadString($"{_config.GetSection("StatesList").Value}");
                    citiesJson = client.DownloadString($"{_config.GetSection("CitiesList").Value}");
                }
                catch (Exception ex) { }
            }

            _countries = JsonConvert.DeserializeObject<List<Country>>(countriesJson);
            _states = JsonConvert.DeserializeObject<List<State>>(statesJson);
            _cities = JsonConvert.DeserializeObject<List<City>>(citiesJson);
        }

        private List<Country> GetCountries()
        {
            foreach (var country in _countries)
            {
                Console.WriteLine($"Creating Country: {country.Name}");

                if (country.Latitude == null || country.Longitude == null)
                {
                    var location = _googleLocationService.GetLatLongFromAddress($"{country.Name}");

                    if (location != null)
                    {
                        country.Latitude = location.Latitude.ToString();
                        country.Longitude = location.Longitude.ToString();
                    }
                }

                if (country.Code == null)
                    country.Code = country.Name;

                country.States = GetStates(country);
            }

            return _countries;
        }

        private List<State> GetStates(Country country)
        {
            var states = _states.Where(s => s.CountryCode == country.Code).ToList();

            foreach (var state in states)
            {
                Console.WriteLine($"\tCreating State: {state.Name}");

                if (state.Latitude == null || state.Longitude == null)
                {
                    var location = _googleLocationService.GetLatLongFromAddress($"{country.Name}, ${state.Name}");

                    if (location != null)
                    {
                        state.Latitude = location.Latitude.ToString();
                        state.Longitude = location.Longitude.ToString();
                    }
                }

                if (state.Code == null)
                    state.Code = state.Name;

                state.Cities = GetCities(country, state);
            }

            return states;
        }

        private List<City> GetCities(Country country, State state)
        {
            var cities = _cities
                .Where(c => c.CountryCode == country.Code && c.StateCode == state.Code)
                .ToList();

            foreach (var city in cities)
            {
                Console.WriteLine($"\t\tCreating City: {city.Name}");

                if (city.Latitude == null || city.Longitude == null)
                {
                    var location = _googleLocationService.GetLatLongFromAddress($"{country.Name}, ${state.Name}, ${city.Name}");

                    if (location != null)
                    {
                        city.Latitude = location.Latitude.ToString();
                        city.Longitude = location.Longitude.ToString();
                    }
                }
            }

            return cities;
        }
    }
}
