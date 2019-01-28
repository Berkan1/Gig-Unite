using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigUnite.Models
{
	public class Comment
	{
		public int Id { get; set; }
		[Required]
		public string Message { get; set; }
		public DateTime TimePosted { get; set; }

		public int UserId { get; set; }

		public int GigId { get; set; }
		public virtual Gig Gig { get; set; }
	}
}
