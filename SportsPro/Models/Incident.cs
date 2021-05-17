using System;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Incident
    {
		public int IncidentID { get; set; }

		[Required]
		public int CustomerID { get; set; }     // foreign key property
		public Customer Customer { get; set; }  // navigation property

		[Required]
		public int ProductID { get; set; }     // foreign key property
		public Product Product { get; set; }   // navigation property

		public int? TechnicianID { get; set; }     // foreign key property - nullable
		public Technician Technician { get; set; }   // navigation property

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[Display(Name = "Date Opened")]
		public DateTime DateOpened { get; set; } = DateTime.Now;

		[Display(Name ="Date Closed")]
		public DateTime? DateClosed { get; set; } = null;
	}
}