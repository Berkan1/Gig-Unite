using GigUnite.DataAccess;
using GigUnite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigUnite.DAO
{
	public static class ReportDAO
	{
		public static int DeleteReport(int reportId)
		{
			string sql = @"DELETE FROM dbo.Report WHERE Id = @ReportId;";

			return SqlDataAccess.DeleteReport(sql, reportId);
		}

		public static void DeleteUser(string userId, int profileId)
		{
			string sql = @"DELETE FROM dbo.AspNetUsers WHERE Id = @ProfileId;";
			string sql2 = @"DELETE FROM dbo.Report WHERE ProfileId = @ProfileId;";
			string sql3 = @"DELETE FROM dbo.ProfileGenre WHERE ProfileId = @ProfileId;";
			string sql4 = @"DELETE FROM dbo.Interest WHERE UserId = @ProfileId;";
			string sql5 = @"DELETE FROM dbo.Gig WHERE ProfileId = @ProfileId;";
			string sql6 = @"DELETE FROM dbo.Profile WHERE Id = @ProfileId;";
			string sql7 = @"DELETE FROM dbo.Comment WHERE UserId = @ProfileId;";

			SqlDataAccess.DeleteUser(sql, userId);
			SqlDataAccess.DeleteUser(sql2, profileId);
			SqlDataAccess.DeleteUser(sql3, profileId);
			SqlDataAccess.DeleteUser(sql4, profileId);
			SqlDataAccess.DeleteUser(sql5, profileId);
			SqlDataAccess.DeleteUser(sql6, profileId);
			SqlDataAccess.DeleteUser(sql7, profileId);
		}
	}
}