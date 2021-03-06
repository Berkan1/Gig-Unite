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
		[Required]
		public string Name { get; set; }

		public virtual ICollection<ProfileGenre> ProfileGenres { get; set; }
		public virtual ICollection<GigGenre> GigGenres { get; set; }
	}
}
