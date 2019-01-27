using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigUnite.Models
{
	public class Gig
	{
		public int Id { get; set; }
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }
		public string Venue { get; set; }
		public decimal Price { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public int BandId { get; set; }
		public virtual Band Band { get; set; }

		public int ProfileId { get; set; }
		public virtual Profile Profile { get; set; }

		public virtual ICollection<Interest> Interests { get; set; }
		public virtual ICollection<Genre> Genres { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
	}
}
