using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigUnite.Models
{
	public class Profile
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string City { get; set; }
		[DataType(DataType.Date)]
		public DateTime Dob { get; set; }
		public string Bio { get; set; }
		public string UserId { get; set; }

		public virtual ICollection<Gig> Gigs { get; set; }
		public virtual ICollection<Interest> Interests { get; set; }
		public virtual ICollection<Genre> Genres { get; set; }
		public virtual ICollection<Band> Bands { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
	}
}
