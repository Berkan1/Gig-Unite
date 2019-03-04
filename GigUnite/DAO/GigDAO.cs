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
	}
}