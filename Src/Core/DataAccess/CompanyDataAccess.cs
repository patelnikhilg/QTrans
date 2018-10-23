using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTrans.Models;

namespace QTrans.DataAccess
{
    public class CompanyDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public CompanyDataAccess()
        {
        }

        ~CompanyDataAccess()
        {
            this.Dispose(false);
        }
        #endregion
        public bool InsertUpdateCompanyDetails(Company company, out long identity, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertUpdateCompany", true))
            {
                connector.AddInParameterWithValue("@CompanyId", company.companyid);
                connector.AddInParameterWithValue("@CompanyName", company.companyname);
                connector.AddInParameterWithValue("@Address", company.address);
                connector.AddInParameterWithValue("@TeleNumber", company.telenumber);
                connector.AddInParameterWithValue("@AlternetTelNumber", company.alternettelnumber);
                connector.AddInParameterWithValue("@Country", company.country);
                connector.AddInParameterWithValue("@State", company.state);
                connector.AddInParameterWithValue("@City", company.city);
                connector.AddInParameterWithValue("@UserId", company.UserId);
                connector.AddInParameterWithValue("@CompanyType", company.companytype);
                connector.AddOutParameterWithType("@identity", SqlDbType.BigInt);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                identity = company.companyid == 0 ? Convert.ToInt64(connector.GetParamaeterValue("@identity")) : company.companyid;
                message = connector.GetParamaeterValue("@Message").ToString();
                               
            }

            return rowEffected > 0;
        }

        public DataTable GetById(long companyId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetCompanyDetailsById", true))
            {                
                connector.AddInParameterWithValue("@CompanyId", companyId);
                connector.AddInParameterWithValue("@userId", 0);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                dt = connector.GetDataTable();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return dt;
        }

        public DataTable GetByUserId(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetCompanyDetailsById", true))
            {
                connector.AddInParameterWithValue("@CompanyId", 0);
                connector.AddInParameterWithValue("@userId", userId);              
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                dt = connector.GetDataTable();
                message = connector.GetParamaeterValue("@Message").ToString();
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
