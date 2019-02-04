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
		[Required]
		[StringLength(30, ErrorMessage = "Name can't exceed 30 characters")]
		[Display(Name = "Display Name")]
		public string Displayname { get; set; }
		[Required]
		[StringLength(30, ErrorMessage = "City can't exceed 30 characters")]
		public string City { get; set; }
		[DataType(DataType.Date)]
		[Display(Name = "Date of Birth")]
		public DateTime Dob { get; set; }
		public string Bio { get; set; }
		public string Band1 { get; set; }
		public string Band2 { get; set; }
		public string Band3 { get; set; }
		public string Band4 { get; set; }
		public string Band5 { get; set; }
		public string ImageURL { get; set; }
		public string UserId { get; set; }

		public virtual ICollection<Gig> Gigs { get; set; }
		public virtual ICollection<ProfileGenre> ProfileGenres { get; set; }
	}
}
