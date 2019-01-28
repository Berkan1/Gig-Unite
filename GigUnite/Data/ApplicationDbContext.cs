using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GigUnite.Models;

namespace GigUnite.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Gig> Gig { get; set; }
		public DbSet<Interest> Interest { get; set; }
		public DbSet<Profile> Profile { get; set; }
		public DbSet<Genre> Genre { get; set; }
		public DbSet<Band> Band { get; set; }
		public DbSet<Comment> Comment { get; set; }
		public DbSet<ProfileGenre> ProfileGenre { get; set; }
		public DbSet<ProfileBand> ProfileBand { get; set; }
		public DbSet<GigGenre> GigGenre { get; set; }

	}
}
