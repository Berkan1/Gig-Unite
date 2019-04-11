using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigUnite.Models
{
	public class Report
	{
		public int Id { get; set; }
		[Required]
		[Display(Name = "Reported by")]
		public string ReportBy { get; set; }
		[Required]
		[Display(Name = "Reason for report")]
		public string Reason { get; set; }

		public int ProfileId { get; set; }
		public virtual Profile Profile { get; set; }
	}
}