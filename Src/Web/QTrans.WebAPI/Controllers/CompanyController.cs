using QTrans.Logging;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    [RoutePrefix("api/company")]
    public class CompanyController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");

        [Route("GetCompanyDetailsById")]
        [HttpGet]
        public IHttpActionResult GetCompanyDetailsById(long userId, long companyId)
        {
            string message = string.Empty;
            using (var companyRepository = new CompanyRepository(userId))
            {
                ResponseSingleModel<Company> result = null;
                if (companyId > 0)
                {
                    result = companyRepository.GetCompanyDetailById(companyId, out message);
                }
                else
                {
                    result = companyRepository.GetCompanyDetailByUserId(userId, out message);
                }

                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("CompanyRegistration")]
        [HttpPost]
        public IHttpActionResult CompanyRegistration([FromBody] Company company)
        {
            string message = string.Empty;
            using (var companyRepository = new CompanyRepository(company.UserId))
            {
                var result = companyRepository.CompanyRegistration(company, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }
    }
}
