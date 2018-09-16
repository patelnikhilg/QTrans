﻿using QTrans.Models;
using System;
using System.Collections.Generic;
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
    }
}
