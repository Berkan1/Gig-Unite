using GigUnite.DataAccess;
using GigUnite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigUnite.DAO
{
	public static class GigDAO
	{
		public static int AddGenresToGig(int gigId, string name)
		{
			string sql = @"SELECT Id FROM dbo.Genre WHERE Name = @Name";

			int theId = SqlDataAccess.GetId(sql, name);

			string sql2 = @"INSERT INTO dbo.GigGenre (GigId, GenreId) VALUES (@FirstId, @GenreId);";

			return SqlDataAccess.SaveIds(sql2, gigId, theId);
		}
		public static int AddComment(int profileId, int gigId, string timePosted, string message)
		{
			string sql = @"INSERT INTO dbo.Comment (Message, TimePosted, UserId, GigId) VALUES (@Message, @TimePosted, @ProfileId, @GigId);";

			return SqlDataAccess.AddComment(sql, profileId, gigId, timePosted, message);
		}

		public static List<string> GetEmailList(string bandName, List<string> genres)
		{
			List<string> recipients = new List<string>();

			foreach (var genre in genres)
			{
				string sql = @"SELECT dbo.AspNetUsers.Email FROM dbo.AspNetUsers 
								INNER JOIN dbo.Profile ON dbo.AspNetUsers.Id=dbo.Profile.UserId 
								INNER JOIN dbo.ProfileGenre ON dbo.Profile.Id=dbo.ProfileGenre.ProfileId
								INNER JOIN dbo.Genre ON dbo.ProfileGenre.GenreId=dbo.Genre.Id WHERE dbo.Genre.Name = @Comparison;";

				recipients.AddRange(SqlDataAccess.GetEmails(sql, genre));
			}

			string sql2 = @"SELECT dbo.AspNetUsers.Email FROM dbo.AspNetUsers 
								INNER JOIN dbo.Profile ON dbo.AspNetUsers.Id=dbo.Profile.UserId
								WHERE UPPER(dbo.Profile.Band1) = @Comparison 
								OR UPPER(dbo.Profile.Band2) = @Comparison 
								OR UPPER(dbo.Profile.Band3) = @Comparison
								OR UPPER(dbo.Profile.Band4) = @Comparison
								OR UPPER(dbo.Profile.Band5) = @Comparison;";

			recipients.AddRange(SqlDataAccess.GetEmails(sql2, bandName.ToUpper()));

			return recipients.Distinct().ToList();
		}
		
		public static int SetInterest(int gigId, int profileId, string level)
		{
			DeleteInterest(gigId, profileId);

			string sql = @"INSERT INTO dbo.Interest (Status, EventId, UserId) VALUES (@Status, @EventId, @UserId);";

			return SqlDataAccess.SetInterest(sql, gigId, profileId, level);
		}

		public static int DeleteInterest(int gigId, int profileId)
		{
			string sql = @"DELETE FROM dbo.Interest WHERE EventId = @EventId AND UserId = @UserId;";

			return SqlDataAccess.DeleteInterest(sql, gigId, profileId);
		}
	}
}