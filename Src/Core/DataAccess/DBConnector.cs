using System;
using System.Data;
using System.Data.SqlClient;

namespace QTrans.DataAccess
{
    internal class DBConnector : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;

        int attemp = byte.MinValue;
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        private string connectionString = @"Data Source=NIK\SQLEXPRESS;Initial Catalog=QTrans;User ID=QTrans;Password=nikhil;";
        public DBConnector(string inlineQuery, bool isSP)
        {
            this.sqlConnection = new SqlConnection(this.connectionString);
            this.sqlCommand = new SqlCommand(inlineQuery, this.sqlConnection)
            {
                CommandType = isSP ? CommandType.StoredProcedure : CommandType.Text
            };
            sqlCommand.Connection.Open();
        }

        public void AddInParameterWithValue(string name, object value)
        {
            SqlParameter param = new SqlParameter(name, value)
            {
                Direction = ParameterDirection.Input
            };
            sqlCommand.Parameters.Add(param);            
        }

        public void AddOutParameterWithValue(string name, object value)
        {
            SqlParameter param = new SqlParameter(name, value)
            {
                Direction = ParameterDirection.Output
            };
            sqlCommand.Parameters.Add(param);
        }

        public void AddInOutParameterWithValue(string name, object value)
        {
            SqlParameter param = new SqlParameter(name, value)
            {
                Direction = ParameterDirection.InputOutput
            };
            sqlCommand.Parameters.Add(param);
        }

        public void AddReturnParameterWithValue(string name, object value)
        {
            SqlParameter param = new SqlParameter(name, value)
            {
                Direction = ParameterDirection.ReturnValue
            };
            sqlCommand.Parameters.Add(param);
        }

        public void AddInParameter(SqlParameter parameter)
        {
            sqlCommand.Parameters.Add(parameter);
        }
        public void AddInParameter(string name, SqlDbType dbType, int size, string sourceColumn)
        {   
            sqlCommand.Parameters.Add(name, dbType, size, sourceColumn);
        }
        public void AddOutParameter(string name, SqlDbType dbType, int size)
        {
            sqlCommand.Parameters.Add(name, dbType, size);
        }

        public object GetParamaeterValue(string parameter)
        {
            return sqlCommand.Parameters[parameter].Value;
        }

        public int ExceuteNonQuery()
        {
            return sqlCommand.ExecuteNonQuery();
        }

        public SqlDataReader ExecuteReader()
        {
            return sqlCommand.ExecuteReader();
        }

        public object ExecuteScalar()
        {
            return sqlCommand.ExecuteScalar();
        }

        public DataTable GetDataTable()
        {
            DataSet ds = null;
            using (var adapter = new SqlDataAdapter(sqlCommand))
            {
                adapter.Fill(ds);
            }

            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        public DataSet GetDataSet()
        {
            DataSet ds = null;
            using (var adapter = new SqlDataAdapter(sqlCommand))
            {
                adapter.Fill(ds);
            }

            return ds;
        }


        /// <summary>
        /// Destructor of Common Execute query class.
        /// </summary>
        ~DBConnector()
        {
            this.Dispose(false);
        }


        /// <summary>
        /// Dispose the resource which is allocated for this object.
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release the all allocated resource.
        /// </summary>
        /// <param name="disposing">Flg for object is already dispose by system</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {
                ////Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }
    }
}
