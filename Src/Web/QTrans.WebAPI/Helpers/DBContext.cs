using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Transport.WebAPI.Helpers
{
	public class DBContext
	{
		public static string ForetakeConnectionString = ConfigurationManager.ConnectionStrings["ForetakeConnectionString"].ConnectionString;


		public static List<T> LoadDataFromReader<T>(string spName, params object[] parameters) where T : new()
		{
			List<T> list = new List<T>();
			using (SqlConnection connection = new SqlConnection(ForetakeConnectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(spName, connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					foreach (SqlParameter param in parameters)
					{
						command.Parameters.Add(param);
					}
					command.CommandTimeout = 600;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								T obj = new T();

								for (int i = 0; i < reader.FieldCount; i++)
								{
									string columnName = reader.GetName(i);
									var property = typeof(T).GetProperty(columnName);
									if ((property != null) && (property.GetSetMethod() != null))
									{
										property.SetValue(obj, reader[columnName] == DBNull.Value ? null : reader[columnName], null);
									}
								}
								list.Add(obj);
							}
						}
					}



				} // command disposed here
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();

			}
			return list;
		}

		public static string ExecuteNonQueryOutParam(string spName, params object[] parameters)
		{
            string response = string.Empty;

            using (SqlConnection connection = new SqlConnection(ForetakeConnectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(spName, connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					foreach (SqlParameter param in parameters)
					{
						command.Parameters.Add(param);
					}
					command.CommandTimeout = 600;
					command.ExecuteNonQuery();
                    response = command.Parameters["@Status"].Value.ToString();

                    connection.Close();
				} // command disposed here
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();

			}

            return response;
		}

        public static void ExecuteNonQuery(string spName, params object[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(ForetakeConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(spName, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (SqlParameter param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                    connection.Close();
                } // command disposed here
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

            }
        }


        public static DataSet ExecuteDataSet(string spName, params object[] parameters)
		{
			DataSet ds = new DataSet();
			using (SqlConnection connection = new SqlConnection(ForetakeConnectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(spName, connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					foreach (SqlParameter param in parameters)
					{
						command.Parameters.Add(param);
					}
					command.CommandTimeout = 600;
					SqlDataAdapter da = new SqlDataAdapter(command);
					da.Fill(ds);
					connection.Close();
				} // command disposed here
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();

			}
			return ds;
		}

		public static object ExecuteScalar(string spName, params object[] parameters)
		{
			object obj;
			using (SqlConnection connection = new SqlConnection(ForetakeConnectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(spName, connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					foreach (SqlParameter param in parameters)
					{
						command.Parameters.Add(param);
					}
					command.CommandTimeout = 600;
					obj = command.ExecuteScalar();
					connection.Close();
				} // command disposed here
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();

			}
			return obj;
		}

		public static List<T> LoadDataFromReader<T>(string connectionString, string spName, params object[] parameters) where T : new()
		{
			List<T> list = new List<T>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(spName, connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					foreach (SqlParameter param in parameters)
					{
						command.Parameters.Add(param);
					}
					command.CommandTimeout = 600;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								T obj = new T();

								for (int i = 0; i < reader.FieldCount; i++)
								{
									string columnName = reader.GetName(i);
									var property = typeof(T).GetProperty(columnName);
									if ((property != null) && (property.GetSetMethod() != null))
									{
										property.SetValue(obj, reader[columnName] == DBNull.Value ? null : reader[columnName], null);
									}
								}
								list.Add(obj);
							}
						}
					}



				} // command disposed here
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();

			}
			return list;
		}

		public static void ExecuteNonQuery(string connectionString, string spName, params object[] parameters)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(spName, connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					foreach (SqlParameter param in parameters)
					{
						command.Parameters.Add(param);
					}
					command.CommandTimeout = 600;
					command.ExecuteNonQuery();
					connection.Close();
				} // command disposed here
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();

			}
		}

		public static DataSet ExecuteDataSet(string connectionString, string spName, params object[] parameters)
		{
			DataSet ds = new DataSet();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(spName, connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					foreach (SqlParameter param in parameters)
					{
						command.Parameters.Add(param);
					}
					command.CommandTimeout = 600;
					SqlDataAdapter da = new SqlDataAdapter(command);
					da.Fill(ds);
					connection.Close();
				} // command disposed here
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();

			}
			return ds;
		}

		public static object ExecuteScalar(string connectionString, string spName, params object[] parameters)
		{
			object obj;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(spName, connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					foreach (SqlParameter param in parameters)
					{
						command.Parameters.Add(param);
					}
					command.CommandTimeout = 600;
					obj = command.ExecuteScalar();
					connection.Close();
				} // command disposed here
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();

			}
			return obj;
		}
	}
}