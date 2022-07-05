using CountriesOftheWorld.Models;
using CountriesOftheWorld.Services;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace CountriesOftheWorld.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ICountries countries = new Countries();

        public ActionResult Index()
        {
            return View();
        }

        // GET: Countries
        public async Task<ActionResult> GetCountries()
        {
            List<CountryInfo> countryList = new List<CountryInfo>();

            //Check if there is data stored in the cache
            if (HttpContext.Cache["AllCountries"] == null)
            {
                //Get the list from the api
                countryList = await countries.GetCountryListAsync();
                HttpContext.Cache.Insert("AllCountries", countryList, null, DateTime.Now.AddDays(1), TimeSpan.Zero);
            }
            else
            {
                //If there is data saved in the cache, store it in the list
                countryList = (List<CountryInfo>)HttpContext.Cache["AllCountries"];
            }

            return Json(new { data = countryList, draw = Request["draw"] }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCountryInfo()
        {
            string countryname = Request["countryname"];

            List<CountryInfo> countryList = new List<CountryInfo>();

            //Store data from cache to list 
            countryList = (List<CountryInfo>)HttpContext.Cache["AllCountries"];

            //Query the list yo get only informatio about the selected country
            countryList = countryList.
                       Where(x => x.name.ToLower().Contains(countryname.ToLower())).ToList();

            return Json(new { data = countryList, draw = Request["draw"] }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetRegions()
        {
            string selectedregion = Request["region"];

            List<CountryInfo> countryList = new List<CountryInfo>();

            //Store data from cache to list
            countryList = (List<CountryInfo>)HttpContext.Cache["AllCountries"];
           
            //Query the list yo get only informatio about the selected country
            countryList = countryList.
                       Where(x => x.region.ToLower().Contains(selectedregion.ToLower())).ToList();

            RegionInfo region = new RegionInfo();

            //To get the sum of the population in the selected region
            int RegionPopulation = countryList.Sum(x => x.population);          

            region.population = RegionPopulation;

            // this will return the selected region
            region.regionname = selectedregion;

            // this will return a list containing all countries in the region
            region.name = countryList.Where(x => x.region.ToLower().Contains(selectedregion.ToLower()))
                .Select(x => x.name).Distinct().ToList();

            // this will return a list containing all subregions in the region
            region.subregion = countryList.Where(x => x.region.ToLower().Contains(selectedregion.ToLower()))
                .Select(x => x.subregion).Distinct().ToList();

           var regionList = JsonConvert.SerializeObject(region);

            return Json(new { data = regionList, draw = Request["draw"] }, JsonRequestBehavior.AllowGet);

        }

    }
}