using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTrans.Models;

namespace QTrans.DataAccess
{
    public class CompanyDataAccess
    {
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
                connector.AddInParameterWithValue("@UserId", company.userid);
                connector.AddInParameterWithValue("@ComanyType", company.comanytype);
                connector.AddOutParameterWithType("@identity", SqlDbType.BigInt);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = Convert.ToInt64(connector.GetParamaeterValue("@identity"));
            }

            return rowEffected > 0;
        }

        public DataTable GetById(long companyId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetCompanyDetailsById", true))
            {                
                connector.AddInParameterWithValue("@CompanyId", companyId);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                dt = connector.GetDataTable();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return dt;
        }
    }
}
