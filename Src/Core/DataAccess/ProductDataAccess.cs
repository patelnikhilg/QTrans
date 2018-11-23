using QTrans.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.DataAccess
{
    public class ProductDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public ProductDataAccess()
        {
        }

        ~ProductDataAccess()
        {
            this.Dispose(false);
        }
        #endregion

        public bool InsertUpdateProduct(Product product, out long identity, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertUpdateProduct", true))
            {
                connector.AddInParameterWithValue("@productId", product.productId);
                connector.AddInParameterWithValue("@UnitId", product.UnitId);
                connector.AddInParameterWithValue("@IMEI", product.IMEI);
                connector.AddInParameterWithValue("@RegisterDate", product.RegisterDate);
                connector.AddInParameterWithValue("@Version", product.Version);
                connector.AddInParameterWithValue("@FirmwareVersion", product.FirmwareVersion);
                connector.AddInParameterWithValue("@WarrentyExpDate", product.WarrentyExpDate);
                connector.AddInParameterWithValue("@Status", product.Status);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = product.productId == 0 ? Convert.ToInt64(connector.GetParamaeterValue("@identity")) : product.productId;
            }

            return rowEffected > 0;
        }

        public bool InsertUpdateInstallProduct(ProductInstall install, out long identity, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertUpdateInstallProduct", true))
            {
                connector.AddInParameterWithValue("@dtlProductId", install.dtlProductId);
                connector.AddInParameterWithValue("@productId", install.productId);
                connector.AddInParameterWithValue("@VehicleId", install.VehicleId);
                connector.AddInParameterWithValue("@InsttalationDate", install.InsttalationDate);
                connector.AddInParameterWithValue("@UnitName", install.UnitName);
                connector.AddInParameterWithValue("@Status", install.Status);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = install.dtlProductId == 0 ? Convert.ToInt64(connector.GetParamaeterValue("@identity")) : install.dtlProductId;
            }

            return rowEffected > 0;
        }

        public DataTable GetById(long productId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetProductById", true))
            {
                connector.AddInParameterWithValue("@productId", productId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetInstallProductById(long DtlProductId,long productId, long vehicleId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetInstallProductById", true))
            {
                connector.AddInParameterWithValue("@productId", productId);
                connector.AddInParameterWithValue("@vehicleId", vehicleId);
                connector.AddInParameterWithValue("@DtlProductId", DtlProductId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetInstallProductListByUserId(long userId,long installer)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetInstallProductListByUserId", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                connector.AddInParameterWithValue("@installer", installer);                
                dt = connector.GetDataTable();
            }
            return dt;
        }

        public DataTable GetProductListByUserId(long userId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetProductListByUserId", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                dt = connector.GetDataTable();
            }
            return dt;
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
