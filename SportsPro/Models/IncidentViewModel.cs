using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class IncidentViewModel
    {
        public List<Incident> Incidents { get; set;  }// incident list
        public string Filter { get; set; }
        public string CheckFilters(string filter) =>   // string that specifies filtering for the page (Linq)
            filter == Filter ? "active" : "";
        public string Title { get; set; }
        public string Customer { get; set; }
        //public DateTime 

    }
}
