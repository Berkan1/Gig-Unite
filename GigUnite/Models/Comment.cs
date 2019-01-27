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
		public string Message { get; set; }
		public DateTime TimePosted { get; set; }

		public int ProfileId { get; set; }
		public virtual Profile Profile { get; set; }

		public int GigId { get; set; }
		public virtual Gig Gig { get; set; }
	}
}
