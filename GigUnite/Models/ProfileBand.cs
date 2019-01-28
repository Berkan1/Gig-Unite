using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigUnite.Models
{
	public class ProfileBand
	{
		public int Id { get; set; }

		public int ProfileId { get; set; }
		public virtual Profile Profile { get; set; }

		public int BandId { get; set; }
		public virtual Band Band { get; set; }
	}
}