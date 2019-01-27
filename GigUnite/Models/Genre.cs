﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigUnite.Models
{
	public class Genre
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public virtual ICollection<Profile> Profiles { get; set; }
		public virtual ICollection<Gig> Gigs { get; set; }
	}
}
