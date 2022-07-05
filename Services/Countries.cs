using CountriesOftheWorld.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CountriesOftheWorld.Services
{
    public class Countries : ICountries
    {
        

       public async Task<List<CountryInfo>> GetCountryListAsync()
        {
            try
            {
                var endpointUrl = ConfigurationManager.AppSettings["CountriesEndpoint"];
                RestClient client = new RestClient(endpointUrl);
                RestRequest request = new RestRequest();
                IRestResponse response = await client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<List<CountryInfo>>(response.Content);
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

    }

}