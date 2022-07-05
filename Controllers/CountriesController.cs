using CountriesOftheWorld.Models;
using CountriesOftheWorld.Services;
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
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            List<CountryInfo> countryList = new List<CountryInfo>();

            if (HttpContext.Cache["AllCountries"] == null)
            {
                countryList = await countries.GetCountryListAsync();
                HttpContext.Cache.Insert("AllCountries", countryList, null, DateTime.Now.AddSeconds(30), TimeSpan.Zero);
            }
            else
            {
                countryList = (List<CountryInfo>)HttpContext.Cache["AllCountries"];
            }
            
            int totalrows = countryList.Count;

            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                countryList = countryList.
                        Where(x => x.name.ToLower().Contains(searchValue.ToLower()) || x.region.ToLower().Contains(searchValue.ToLower()) || x.subregion.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfiltering = countryList.Count;

            //sorting
            countryList = countryList.OrderBy(s => sortColumnName).ToList();


            return Json(new { data = countryList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCountryInfo()
        {
            string countryname = Request["countryname"];

            List<CountryInfo> countryList = new List<CountryInfo>();

            countryList = (List<CountryInfo>)HttpContext.Cache["AllCountries"];

            countryList = countryList.
                       Where(x => x.name.ToLower().Contains(countryname.ToLower())).ToList();

            return Json(new { data = countryList, draw = Request["draw"] }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetRegions()
        {
            string region = Request["region"];

            List<CountryInfo> countryList = new List<CountryInfo>();

            countryList = (List<CountryInfo>)HttpContext.Cache["AllCountries"];

            countryList = countryList.
                       Where(x => x.name.ToLower().Contains(region.ToLower())).ToList();

            return Json(new { data = countryList, draw = Request["draw"] }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSubregions()
        {
            string subregion = Request["subregion"];

            List<CountryInfo> countryList = new List<CountryInfo>();

            countryList = (List<CountryInfo>)HttpContext.Cache["AllCountries"];

            countryList = countryList.
                       Where(x => x.name.ToLower().Contains(subregion.ToLower())).ToList();

            return Json(new { data = countryList, draw = Request["draw"] }, JsonRequestBehavior.AllowGet);

        }
    }
}