using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SportsPro.Models
{
    public class Customer
    {
		public int CustomerID { get; set; }

		[Required(ErrorMessage = "Please enter First name.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Please enter Last name.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Please enter an Address.")]
		public string Address { get; set; }

		[Required(ErrorMessage = "Please enter a City.")]
		public string City { get; set; }

		[Required(ErrorMessage = "Please enter a State.")]
		public string State { get; set; }

		[Required(ErrorMessage = "Please enter a Postal Code.")]
		public string PostalCode { get; set; }
				
		public string CountryID { get; set; }

		public Country Country { get; set; }

		[RegularExpression("\\D*([2-9]\\d{2})(\\D*)([2-9]\\d{2})(\\D*)(\\d{4})\\D*", ErrorMessage = "Please input in format 0000000000")]
		public string Phone { get; set; }

		public string Email { get; set; }

		public string FullName => FirstName + " " + LastName;   // read-only property

		public ICollection<Registration> Registrations { get; set; } //navigation property to linking entity
	}
}