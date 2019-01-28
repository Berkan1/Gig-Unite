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

		public int GigId { get; set; }
		public int ProfileId { get; set; }
		
		public virtual Gig Gig { get; set; }
		public virtual Profile Profile { get; set; }
	}
}