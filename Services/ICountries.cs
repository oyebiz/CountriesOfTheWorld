using CountriesOftheWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesOftheWorld.Services
{
    public interface ICountries
    {
        Task<List<CountryInfo>> GetCountryListAsync();
        Task<List<CountryInfo>> GetCountryInfoAsync(string name);
    }
}
