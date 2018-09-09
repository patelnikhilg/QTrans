using System;
using System.Collections.Generic;
using System.Text;
using QTrans.DataAccess;
using QTrans.Models;

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

        public Company CompanyRegistration(Company company, out string message)
        {
            long companyId = 0;
            message = string.Empty;
            Company companyDetail = null;
            if (this.instanceCompany.InsertUpdateCompanyDetails(company, out companyId, out message))
            {
                companyDetail.companyid = companyId;
                //var dt = this.instanceCompany.GetById(companyId, out message);
                //var lst = DataAccessUtility.ConvertToList<Company>(dt);
                //companyDetail = lst.Count > 0 ? lst[0] : null;
            }

            return companyDetail;
        }

        public Company GetCompanyDetailById(long companyId, out string message)
        {
            message = string.Empty;
            Company companyDetail = null;
            companyDetail.companyid = companyId;
            var dt = this.instanceCompany.GetById(companyId, out message);
            var lst = DataAccessUtility.ConvertToList<Company>(dt);
            companyDetail = lst.Count > 0 ? lst[0] : null;
            return companyDetail;
        }
    }
}
