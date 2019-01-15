using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.DataAccess
{
    public class TrackingDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public TrackingDataAccess()
        {
        }

        ~TrackingDataAccess()
        {
            this.Dispose(false);
        }
        #endregion

        public bool BulkInsertion(DataTable table)
        {
            int rowEffected = 0;
            try
            {
                using (DBConnector connector = new DBConnector("Usp_InsertLocationDetails", true))
                {
                    connector.AddInTableParameterWithValue("@locationDetails", table);                    
                    rowEffected = connector.ExceuteNonQuery();
                }
            }
            catch
            {
                throw;
            }

            return rowEffected > 0;
        }

        #region ========================= Dispose Method ==============
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {

                ////TODO: Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        #endregion
    }
}
