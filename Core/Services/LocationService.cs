using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _db;

        public LocationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Country CreateOrUpdateCountry(Country country)
        {
            var existingCountry = GetCountry(country.Name);

            if (existingCountry != null)
            {
                existingCountry.Name = country.Name;
                existingCountry.Code = country.Code;
                existingCountry.States = country.States;
                existingCountry.Latitude = country.Latitude;
                existingCountry.Longitude = country.Longitude;

                existingCountry.Updated = DateTime.UtcNow;

                _db.Countries.Update(existingCountry);
                _db.SaveChanges();

                return existingCountry;
            }
            else
            {
                country.Created = DateTime.UtcNow;

                _db.Countries.Add(country);
                _db.SaveChanges();

                return country;
            }
        }

        public void DeleteCountry(Country country)
        {
            _db.Countries.Remove(country);
            _db.SaveChanges();
        }

        public State CreateOrUpdateState(State state)
        {
            var existingState = GetState(state.CountryCode, state.Name);

            if (existingState != null)
            {
                existingState.Name = state.Name;
                existingState.Code = state.Code;
                existingState.CountryCode = state.CountryCode;
                existingState.Cities = state.Cities;
                existingState.Latitude = state.Latitude;
                existingState.Longitude = state.Longitude;

                existingState.Updated = DateTime.UtcNow;

                _db.States.Update(existingState);
                _db.SaveChanges();

                return existingState;
            }
            else
            {
                state.Created = DateTime.UtcNow;

                _db.States.Add(state);
                _db.SaveChanges();

                return state;
            }
        }

        public void DeleteState(State state)
        {
            _db.States.Remove(state);
            _db.SaveChanges();
        }

        public List<Country> GetCountries()
        {
            var countries = _db.Countries.ToList();

            return countries;
        }

        public List<Country> GetCountriesWithStates()
        {
            var countries = _db.Countries.Include(c => c.States).ToList();

            return countries;
        }

        //public List<Country> GetCountriesWithStatesAndCities()
        //{
        //    var countries = _db.Countries.Include(c => c.States).ToList();

        //    return countries;
        //}

        public Country GetCountry(string countryCode)
        {
            var countries = GetCountries();
            var country = countries.FirstOrDefault(c => c.Code.ToUpper() == countryCode.ToUpper());

            return country;
        }

        public List<State> GetStates(string countryCode)
        {
            var country = GetCountry(countryCode);

            return country.States;
        }

        public State GetState(string countryCode, string stateCode)
        {
            var states = GetStates(countryCode);
            var state = states.FirstOrDefault(s => s.Code.ToUpper() == stateCode.ToUpper());

            return state;
        }

        public List<City> GetCities(string countryCode, string stateCode)
        {
            var state = GetState(countryCode, stateCode);

            return state.Cities;
        }

        public City GetCity(string countryCode, string stateCode, string cityName)
        {
            var cities = GetCities(countryCode, stateCode);
            var city = cities.FirstOrDefault(c => c.Name.ToUpper() == cityName.ToUpper());

            return city;
        }
    }
}
