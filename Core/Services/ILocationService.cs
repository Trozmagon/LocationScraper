using Core.Models;
using System.Collections.Generic;

namespace Bloop.Core.Services.Interfaces
{
    public interface ILocationService
    {
        Country CreateOrUpdateCountry(Country country);
        void DeleteCountry(Country country);
        State CreateOrUpdateState(State state);
        void DeleteState(State country);

        List<Country> GetCountries();
        List<Country> GetCountriesWithStates();
        //List<Country> GetCountriesWithStatesAndCities();
        Country GetCountry(string countryName);
        List<State> GetStates(string countryName);
        State GetState(string countryName, string stateName);
        List<City> GetCities(string countryName, string stateName);
        City GetCity(string countryName, string stateName, string cityName);
    }
}
