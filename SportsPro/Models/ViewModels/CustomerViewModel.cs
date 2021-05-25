using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class CustomerViewModel
    {
        public List<Registration> Registrations { get; set; }

        public List<Product> Products { get; set; }

        public Customer Customer { get; set; }

        public Product Product { get; set; }
    }
}