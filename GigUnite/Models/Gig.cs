﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigUnite.Models
{
	public class Gig
	{
		public int Id { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "Band name can't exceed 50 characters")]
		[Display(Name = "Artist/Band")]
		public string Band { get; set; }
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "Venue name can't exceed 50 characters")]
		public string Venue { get; set; }
		public decimal? Price { get; set; }
		[Url]
		[Required]
		[Display(Name = "Ticket link")]
		public string TicketLink { get; set; }
		[Display(Name = "Number of views")]
		public int Views { get; set; }

		public int ProfileId { get; set; }
		public virtual Profile Profile { get; set; }
		
		public virtual ICollection<GigGenre> GigGenres { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
	}
}
