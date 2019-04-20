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
		public static List<int> LoadRecommendedGigs(int profileId, List<string> genres)
		{
			List<int> gigIds = new List<int>();
			List<string> bands = new List<string>();

			foreach (string genre in genres)
			{
				string sql = @"SELECT dbo.Gig.Id from dbo.Gig
							INNER JOIN dbo.GigGenre ON dbo.Gig.Id=dbo.GigGenre.GigId
							INNER JOIN dbo.Genre ON dbo.GigGenre.GenreId=dbo.Genre.Id
							WHERE dbo.Gig.ProfileId <> @ProfileId AND dbo.Genre.Name = @Genre;";
				gigIds.AddRange(SqlDataAccess.RecommendedGigs(sql, profileId, genre));
			}

			string sql2 = @"SELECT Band1, Band2, Band3, Band4, Band5 from dbo.Profile
							WHERE dbo.Profile.Id = @ProfileId;";
			bands.AddRange(SqlDataAccess.ProfileBands(sql2, profileId));

			foreach (string band in bands)
			{
				string sql3 = @"SELECT dbo.Gig.Id from dbo.Gig WHERE dbo.Gig.Band = @Band
								AND dbo.Gig.ProfileId <> @ProfileId;";
				gigIds.AddRange(SqlDataAccess.RecommendedBands(sql3, profileId, band));
			}
			
			string sql4 = @"SELECT dbo.Gig.Id from dbo.Gig
							INNER JOIN dbo.Interest ON dbo.Gig.Id=dbo.Interest.EventId
							WHERE dbo.Interest.UserId = @ProfileId;";
			List<int> interestedGigs = (SqlDataAccess.InterestedGigs(sql4, profileId));

			return gigIds.Except(interestedGigs).ToList();
		}
	}
}