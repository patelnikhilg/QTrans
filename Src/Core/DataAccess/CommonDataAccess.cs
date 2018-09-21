using QTrans.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.DataAccess
{
    public class CommonDataAccess
    {
        public bool InsertContactDetails(Contact contact)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertContact", true))
            {
                connector.AddInParameterWithValue("@Name", contact.Name);
                connector.AddInParameterWithValue("@MobileNo", contact.MobileNo);
                connector.AddInParameterWithValue("@EmailAddress ", contact.emailaddress);
                connector.AddInParameterWithValue("@State", contact.state);
                connector.AddInParameterWithValue("@city", contact.city);
                connector.AddInParameterWithValue("@Message", contact.Message);
                rowEffected = connector.ExceuteNonQuery();
            }

            return rowEffected > 0;
        }

        public DataTable GetMaterialType()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetMaterialType", true))
            {
                dt = connector.GetDataTable();
            }

            return dt;
        }


        public DataTable GetPackageType()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPackageType", true))
            {
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetVehicleType()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetVehicleType", true))
            {
                dt = connector.GetDataTable();
            }

            return dt;
        }

        #region =========== State,City,Pincode==============
        public DataTable GetState()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetState", true))
            {
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetCity()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetCity", true))
            {
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetPincode()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPincode", true))
            {
                dt = connector.GetDataTable();
            }

            return dt;
        }
        #endregion
    }
}
