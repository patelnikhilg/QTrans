using System;
using System.Collections.Generic;
using System.Text;
using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ResponseModel;

namespace QTrans.Repositories
{
    public class CompanyRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private CompanyDataAccess instance;
        private long UserId;
        #region "=================== Constructor =============================="
        public CompanyRepository(long userid)
        {
            this.UserId = userid;
            this.instance = new CompanyDataAccess();
        }

        ~CompanyRepository()
        {
            this.Dispose(false);
        }
        #endregion

        public ResponseSingleModel<Company> CompanyRegistration(Company company, out string message)
        {
            var result = new ResponseSingleModel<Company>();
            long companyId = 0;
            message = string.Empty;
            if (this.instance.InsertUpdateCompanyDetails(company, out companyId, out message))
            {
                company.companyid = companyId;
                result.Status = Constants.WebApiStatusOk;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }
            result.Response = company;

            return result;
        }

        public ResponseSingleModel<Company> GetCompanyDetailById(long companyId, out string message)
        {
            var result = new ResponseSingleModel<Company>();
            message = string.Empty;
            if (companyId > 0)
            {
                var dt = this.instance.GetById(companyId, out message);
                var lst = DataAccessUtility.ConvertToList<Company>(dt);
                result.Response = lst != null && lst.Count > 0 ? lst[0] : null;
                result.Status = lst != null && lst.Count > 0 ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
                result.Message = message;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = "Company Id is not supply.";
            }

            return result;
        }

        public ResponseSingleModel<Company> GetCompanyDetailByUserId(long userId, out string message)
        {
            var result = new ResponseSingleModel<Company>();
            message = string.Empty;
            var dt = this.instance.GetByUserId(userId, out message);
            var lst = DataAccessUtility.ConvertToList<Company>(dt);
            result.Response = lst != null && lst.Count > 0 ? lst[0] : null;
            result.Status = lst != null && lst.Count > 0 ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
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
                if (this.instance != null)
                {
                    this.instance.Dispose();
                    this.instance = null;
                }

                ////Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }
        #endregion
    }
}
