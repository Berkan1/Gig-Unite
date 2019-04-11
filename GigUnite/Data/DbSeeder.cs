using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigUnite.Models;

namespace GigUnite.Data
{
	public static class DbSeeder
	{
		public static void SeedDb(ApplicationDbContext context)
		{
			context.Database.EnsureCreated();
			if (!context.Genre.Any())
			{
				context.Genre.AddRange(
				new Genre() { Name = "Hip-Hop" },
				new Genre() { Name = "Pop" },
				new Genre() { Name = "Rock" },
				new Genre() { Name = "Rap" },
				new Genre() { Name = "RnB" },
				new Genre() { Name = "Indie" },
				new Genre() { Name = "Drum and Bass" },
				new Genre() { Name = "House" },
				new Genre() { Name = "Alternative Rock" },
				new Genre() { Name = "Electronic" },
				new Genre() { Name = "Classical" },
				new Genre() { Name = "Jazz" },
				new Genre() { Name = "Dance" },
				new Genre() { Name = "Folk" },
				new Genre() { Name = "Reggae" },
				new Genre() { Name = "Heavy Metal" },
				new Genre() { Name = "Soul" },
				new Genre() { Name = "Punk Rock" },
				new Genre() { Name = "Industrial" },
				new Genre() { Name = "Shoegaze" },
				new Genre() { Name = "Progressive" },
				new Genre() { Name = "Dubstep" },
				new Genre() { Name = "Art" },
				new Genre() { Name = "Acoustic" },
				new Genre() { Name = "Country" },
				new Genre() { Name = "Latin" }
				);
				context.SaveChanges();
			}
		}
	}
}
