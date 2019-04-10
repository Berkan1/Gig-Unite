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
using GigUnite.Models;

namespace GigUnite.DataAccess
{
	public static class SqlDataAccess
	{
		static string connect = "Server=(localdb)\\mssqllocaldb;Database=aspnet-GigUnite-B45964CB-01FC-4D77-A0CD-8AE3770FE19E;Trusted_Connection=True;MultipleActiveResultSets=true";

		public static List<Gig> LoadData(string sql, string genre)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@Genre", SqlDbType.NVarChar);
				command.Parameters["@Genre"].Value = genre;
				List<Gig> gigs = new List<Gig>();

				try
				{
					cnn.Open();
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						gigs.Add(new Gig
						{
							Id = (int)reader["Id"],
							Band = (string)reader["Band"],
							Date = (DateTime)reader["Date"],
							Venue = (string)reader["Venue"],
							Price = (decimal)reader["Price"],
							ProfileId = (int)reader["ProfileId"]
						});
					}
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}

				return gigs;
			}
		}

		public static int GetId(string sql, string name)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@Name", SqlDbType.NVarChar);
				command.Parameters["@Name"].Value = name;
				int result = 0;

				try
				{
					cnn.Open();
					result = (int)command.ExecuteScalar();
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
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
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
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

		public static int SaveIds(string sql, int profileId, int genreId)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@FirstId", SqlDbType.NVarChar);
				command.Parameters["@FirstId"].Value = profileId;
				command.Parameters.Add("@GenreId", SqlDbType.NVarChar);
				command.Parameters["@GenreId"].Value = genreId;
				int result;

				try
				{
					cnn.Open();
					result = command.ExecuteNonQuery();
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}
		}

		public static int DeleteId(string sql, int profileId)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@ProfileId", SqlDbType.NVarChar);
				command.Parameters["@ProfileId"].Value = profileId;
				int result;

				try
				{
					cnn.Open();
					result = command.ExecuteNonQuery();
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}
		}

		public static int Exists(string sql, string condition)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@Displayname", SqlDbType.NVarChar);
				command.Parameters["@Displayname"].Value = condition;
				int result = 0;

				try
				{
					cnn.Open();
					result = (int)command.ExecuteScalar();
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}
		}

		public static int Delete(string sql, string sql2, string sql3, string sql4, string userId)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@UserId", SqlDbType.NVarChar);
				command.Parameters["@UserId"].Value = userId;
				int result = 0;

				try
				{
					cnn.Open();
					result = (int)command.ExecuteScalar();
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}

				DeleteProfile(sql2, result);

				DeleteId(sql3, result);

				DeleteInterest(sql4, result);

				return result;
			}
		}

		public static int DeleteProfile(string sql, int Id)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@Id", SqlDbType.NVarChar);
				command.Parameters["@Id"].Value = Id;
				int result;

				try
				{
					cnn.Open();
					result = command.ExecuteNonQuery();
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}
		}

		public static int DeleteInterest(string sql, int UserId)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@UserId", SqlDbType.NVarChar);
				command.Parameters["@UserId"].Value = UserId;
				int result;

				try
				{
					cnn.Open();
					result = command.ExecuteNonQuery();
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}
		}

		public static int AddComment(string sql, int profileId, int gigId, string timePosted, string message)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@ProfileId", SqlDbType.NVarChar);
				command.Parameters["@ProfileId"].Value = profileId;
				command.Parameters.Add("@GigId", SqlDbType.NVarChar);
				command.Parameters["@GigId"].Value = gigId;
				command.Parameters.Add("@Message", SqlDbType.NVarChar);
				command.Parameters["@Message"].Value = message;
				command.Parameters.Add("@TimePosted", SqlDbType.NVarChar);
				command.Parameters["@TimePosted"].Value = timePosted;
				int result;

				try
				{
					cnn.Open();
					result = command.ExecuteNonQuery();
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}
		}

		public static List<string> GetEmails(string sql, string comparison)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@Comparison", SqlDbType.NVarChar);
				command.Parameters["@Comparison"].Value = comparison;
				List<string> recipients = new List<string>();

				try
				{
					cnn.Open();
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						string email = (string)reader["Email"];
						recipients.Add(email);
					}
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}

				return recipients;
			}
		}

		public static int SetInterest(string sql, int gigId, int profileId, string level)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@Status", SqlDbType.NVarChar);
				command.Parameters["@Status"].Value = level;
				command.Parameters.Add("@EventId", SqlDbType.NVarChar);
				command.Parameters["@EventId"].Value = gigId;
				command.Parameters.Add("@UserId", SqlDbType.NVarChar);
				command.Parameters["@UserId"].Value = profileId;
				int result;

				try
				{
					cnn.Open();
					result = command.ExecuteNonQuery();
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}
		}

		public static int DeleteInterest(string sql, int gigId, int profileId)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@EventId", SqlDbType.NVarChar);
				command.Parameters["@EventId"].Value = gigId;
				command.Parameters.Add("@UserId", SqlDbType.NVarChar);
				command.Parameters["@UserId"].Value = profileId;
				int result;

				try
				{
					cnn.Open();
					result = command.ExecuteNonQuery();
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}
		}

		public static List<int> RecommendedGigs(string sql, string genre)
		{
			using (SqlConnection cnn = new SqlConnection(connect))
			{
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.Add("@Genre", SqlDbType.NVarChar);
				command.Parameters["@Genre"].Value = genre;
				List<int> gigIds = new List<int>();

				try
				{
					cnn.Open();
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						int gigId = (int)reader["Id"];
						gigIds.Add(gigId);
					}
					cnn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}

				return gigIds;
			}
		}
	}
}
