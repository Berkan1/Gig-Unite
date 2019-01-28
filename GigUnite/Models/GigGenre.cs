using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigUnite.Models
{
	public class GigGenre
	{
		public int Id { get; set; }

		public int GigId { get; set; }
		public virtual Gig Gig { get; set; }

		public int GenreId { get; set; }
		public virtual Genre Genre { get; set; }
	}
}