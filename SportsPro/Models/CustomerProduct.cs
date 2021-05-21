using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class CustomerProduct
    {
        //composite primary key
        public int CustomerID { get; set; } //foreign key for customer
        public int ProductID { get; set; } //foreign key for product

        //navigation properties
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
