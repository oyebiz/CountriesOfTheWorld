using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CountriesOftheWorld.Models
{
    public class RegionInfo
    {    
            public string region { get; set; }
            public int population { get; set; }
            public List<string> name { get; set; }
            public List<string> subregion { get; set; }
    }
}