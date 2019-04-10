using GigUnite.DataAccess;
using GigUnite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigUnite.DAO
{
	public static class HomeDAO
	{
		public static List<int> LoadGigs(List<string> genres)
		{
			List<int> gigIds = new List<int>();

			foreach (string genre in genres)
			{
				string sql = @"SELECT dbo.Gig.Id from dbo.Gig
							INNER JOIN dbo.GigGenre ON dbo.Gig.Id=dbo.GigGenre.GigId
							INNER JOIN dbo.Genre ON dbo.GigGenre.GenreId=dbo.Genre.Id
							WHERE dbo.Genre.Name = @Genre;";
				gigIds.AddRange(SqlDataAccess.RecommendedGigs(sql, genre));
			}

			return gigIds;
		}
	}
}