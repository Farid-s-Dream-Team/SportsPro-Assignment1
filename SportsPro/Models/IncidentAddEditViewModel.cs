using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class IncidentAddEditViewModel
    {
        
        public List<Customer> Customers { get; set; }// list of customers
        public List<Product> Products { get; set; }// list of products
        public List<Technician> Technicians { get; set; }//list of technicians
        public Incident currentIncedent { get; set; }//curent incident (prop type incident)
        public string SelectedAction { get; set; }
        public string checkAction(string action) =>
            action == SelectedAction ? "edit" : "add";
    }
}
