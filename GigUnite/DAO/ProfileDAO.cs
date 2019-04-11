using GigUnite.DataAccess;
using GigUnite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigUnite.DAO
{
	public static class ProfileDAO
	{
		public static int CreateProfile(string displayname, DateTime dob, string userId)
		{
			Profile data = new Profile
			{
				Displayname = displayname,
				Dob = dob,
				UserId = userId
			};

			string sql = @"INSERT INTO dbo.Profile (Displayname, Dob, UserId) VALUES (@Displayname, @Dob, @UserId);";

			return SqlDataAccess.SaveData(sql, data);
		}

		public static int LoadYourProfile(string userId)
		{
			string sql = @"IF EXISTS(SELECT Id, Displayname, Dob, Bio, ImageURL, UserId FROM dbo.Profile WHERE UserId = @UserId) SELECT 1 ELSE SELECT 0;";

			return SqlDataAccess.LoadUserData(sql, userId);
		}

		public static int DeleteProfileGenres(int profileId)
		{
			string deletesql = @"DELETE FROM dbo.ProfileGenre WHERE ProfileId = @ProfileId";

			return SqlDataAccess.DeleteId(deletesql, profileId);
		}

		public static int AddGenresToProfile(int profileId, string name)
		{
			string sql = @"SELECT Id FROM dbo.Genre WHERE Name = @Name";

			int theId = SqlDataAccess.GetId(sql, name);

			string sql2 = @"INSERT INTO dbo.ProfileGenre (ProfileId, GenreId) VALUES (@FirstId, @GenreId);";

			return SqlDataAccess.SaveIds(sql2, profileId, theId);
		}

		public static int CheckNameAvailability(string displayname)
		{
			string sql = @"SELECT COUNT(*) FROM dbo.Profile WHERE Displayname = @Displayname;";

			return SqlDataAccess.Exists(sql, displayname);
		}

		public static int deleteProfile(string userId)
		{
			string sql = @"SELECT Id FROM dbo.Profile WHERE UserId = @UserId";
			string sql2 = @"DELETE FROM dbo.Profile WHERE Id = @Id;";
			string sql3 = @"DELETE FROM dbo.ProfileGenre WHERE ProfileId = @ProfileId;";
			string sql4 = @"DELETE FROM dbo.Interest WHERE UserId = @UserId;";

			return SqlDataAccess.Delete(sql, sql2, sql3, sql4, userId);
		}
	}
}