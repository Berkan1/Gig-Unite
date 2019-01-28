using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigUnite.Models
{
	public class Interest
	{
		public int Id { get; set; }
		[Required]
		public string Status { get; set; }

		public int EventId { get; set; }

		public int UserId { get; set; }
	}
}