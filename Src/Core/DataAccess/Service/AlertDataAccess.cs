using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.DataAccess.Service
{
    public class AlertDataAccess
    {
        public DataTable GetPostAlert(int batchSize)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostAlert", true))
            {
                connector.AddInParameterWithValue("@BatchSize", batchSize);
                dt = connector.GetDataTable();
            }

            return dt;
        }
    }
}
