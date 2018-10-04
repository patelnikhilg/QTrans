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
            if (this.instanceCompany.InsertUpdateCompanyDetails(company, out companyId, out message))
            {
                company.companyid = companyId;
            }

            return company;
        }

        public Company GetCompanyDetailById(long companyId, out string message)
        {
            message = string.Empty;
            Company companyDetail = null;
            if (companyId > 0)
            {
                var dt = this.instanceCompany.GetById(companyId, out message);
                var lst = DataAccessUtility.ConvertToList<Company>(dt);
                companyDetail = lst.Count > 0 ? lst[0] : null;
            }
            return companyDetail;
        }

        public Company GetCompanyDetailByUserId(long userId, out string message)
        {
            message = string.Empty;
            var dt = this.instanceCompany.GetByUserId(userId, out message);
            var lst = DataAccessUtility.ConvertToList<Company>(dt);
            Company companyDetail = lst.Count > 0 ? lst[0] : null;
            return companyDetail;
        }
    }
}
