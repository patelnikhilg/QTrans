using QTrans.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Repositories.Repositories
{
    public class TrackingRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private TrackingDataAccess instance;
        #region "=================== Constructor =============================="
        public TrackingRepository()
        {
            instance = new TrackingDataAccess(); 
        }

        ~TrackingRepository()
        {
            this.Dispose(false);
        }
        #endregion


        internal bool BulkInsertion(DataTable dataTable)
        {
            return instance.BulkInsertion(dataTable);
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
