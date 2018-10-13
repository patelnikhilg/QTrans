using System;
using System.Collections.Generic;
using System.Text;
using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ResponseModel;

namespace QTrans.Repositories
{
    public class CompanyRepository
    {
        private CompanyDataAccess instanceCompany;
        private long UserId;
        public CompanyRepository(long userid)
        {
            this.UserId = userid;
            this.instanceCompany = new CompanyDataAccess();
        }

        public ResponseSingleModel<Company> CompanyRegistration(Company company, out string message)
        {
            var result = new ResponseSingleModel<Company>();
            long companyId = 0;
            message = string.Empty;            
            if (this.instanceCompany.InsertUpdateCompanyDetails(company, out companyId, out message))
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
                var dt = this.instanceCompany.GetById(companyId, out message);
                var lst = DataAccessUtility.ConvertToList<Company>(dt);
                result.Response = lst !=null && lst.Count > 0 ? lst[0] : null;
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
            var dt = this.instanceCompany.GetByUserId(userId, out message);
            var lst = DataAccessUtility.ConvertToList<Company>(dt);
            result.Response = lst != null && lst.Count > 0 ? lst[0] : null;
            result.Status = lst != null && lst.Count > 0 ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
    }
}
