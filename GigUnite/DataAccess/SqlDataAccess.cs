using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GigUnite.DataAccess
{
	public static class SqlDataAccess
	{
		static string connect = "Server=(localdb)\\mssqllocaldb;Database=aspnet-GigUnite-B45964CB-01FC-4D77-A0CD-8AE3770FE19E;Trusted_Connection=True;MultipleActiveResultSets=true";

		public static List<T> LoadData<T>(string sql)
		{
			using (IDbConnection cnn = new SqlConnection(connect))
			{
				return cnn.Query<T>(sql).ToList();
			}
		}

		public static int LoadUserData(string sql, string userId)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@UserId", SqlDbType.NVarChar);
				command.Parameters["@UserId"].Value = userId;
				int count;

				try
				{
					cnn.Open();
					count = (int)command.ExecuteScalar();
				}
				catch (Exception ex)
				{
					throw;
				}
				return count;
			}
		}

		public static int SaveData<T>(string sql, T data)
		{
			using (IDbConnection cnn = new SqlConnection(connect))
			{
				return cnn.Execute(sql, data);
			}
		}
	}
}
