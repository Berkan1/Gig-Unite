using GigUnite.DataAccess;
using GigUnite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigUnite.BusinessLogic
{
	public static class ProfileProcessor
	{
		public static int CreateProfile(string displayname, string city, DateTime dob, string userId)
		{
			Profile data = new Profile
			{
				Displayname = displayname,
				City = city,
				Dob = dob,
				UserId = userId
			};

			string sql = @"INSERT INTO dbo.Profile (Displayname, City, Dob, UserId) VALUES (@Displayname, @City, @Dob, @UserId);";

			return SqlDataAccess.SaveData(sql, data);
		}

		public static int LoadYourProfile(string userId)
		{
			string sql = @"IF EXISTS(SELECT Id, Displayname, City, Dob, Bio, ImageURL, UserId FROM dbo.Profile WHERE UserId = @UserId) SELECT 1 ELSE SELECT 0;";

			return SqlDataAccess.LoadUserData(sql, userId);
		}
	}
}
