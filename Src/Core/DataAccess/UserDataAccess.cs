using System.Data;
using QTrans.Models;
using System.Linq;
using System;

namespace QTrans.DataAccess
{
    public class UserDataAccess
    {
        public bool InsertUpdateUserDetails(UserProfile user, out long identity, out string message)
        {
            int rowEffected = 0;
            identity = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertUpdateUser", true))
            {
                connector.AddInParameterWithValue("@emailaddress", user.emailaddress);
                connector.AddInParameterWithValue("@UserId", user.userid);
                connector.AddInParameterWithValue("@FirstName", user.firstname);
                connector.AddInParameterWithValue("@MiddleName", user.middlename);
                connector.AddInParameterWithValue("@LastName", user.lastname);
                connector.AddInParameterWithValue("@MobileNumber", user.mobilenumber);
                connector.AddInParameterWithValue("@LandLineNumber", user.landlinenumber);
                connector.AddInParameterWithValue("@DOB", user.dob);
                connector.AddInParameterWithValue("@AddressLine1", user.addressline1);
                connector.AddInParameterWithValue("@AddressLine2", user.addressline2);
                connector.AddInParameterWithValue("@PinCode", user.pincode);
                connector.AddInParameterWithValue("@photo", user.photo);
                connector.AddInParameterWithValue("@Country", user.country);
                connector.AddInParameterWithValue("@State", user.state);
                connector.AddInParameterWithValue("@District", user.district);
                connector.AddInParameterWithValue("@City", user.city);
                connector.AddInParameterWithValue("@Area", user.area);
                connector.AddInParameterWithValue("@PAN", user.pan);
                connector.AddInParameterWithValue("@GST", user.gst);
                connector.AddInParameterWithValue("@AadhaarNo", user.aadhaarno);
                connector.AddInParameterWithValue("@OTP", user.OTP);
                connector.AddInParameterWithValue("@password", user.Password);
                //  connector.AddInParameterWithValue("@AreaPreferenceDetails", DataAccessUtility.ToDataTable<AreaPreference>(user.areaPreferences.ToList()));
                connector.AddOutParameterWithType("@identity", SqlDbType.BigInt);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = Convert.ToInt64(connector.GetParamaeterValue("@identity"));

            }

            return rowEffected > 0;
        }

        public bool InsertUserCompanyMapping(long userId, long companyId,int TransportTypeId, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertUserCompanyMapping", true))
            {
                connector.AddInParameterWithValue("@UserId", userId);
                connector.AddInParameterWithValue("@CompanyId", companyId);
                connector.AddInParameterWithValue("@TransportTypeId", TransportTypeId);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();                
            }

            return rowEffected > 0;
        }

        public DataTable GetById(long id, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetUserDetailsById", true))
            {                
                connector.AddInParameterWithValue("@UserId", id);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                dt=connector.GetDataTable();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return dt;
        }

        public DataTable GetBytoken(string token, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetUserDetailsBytoken", true))
            {
                connector.AddInParameterWithValue("@token", token);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                dt = connector.GetDataTable();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return dt;
        }

        public bool UpdateMobileEmailVerification(long userid,int OTP, bool isMobile, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_UpdateVerificationById", true))
            {
                connector.AddInParameterWithValue("@UserId", userid);
                connector.AddInParameterWithValue("@OTP", OTP);                
                connector.AddInParameterWithValue("@IsMobile", isMobile); // if true means mobile verification otherwise email verification
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();              
            }

            return rowEffected > 0;
        }

        public bool UpdateToken(long userId,string token, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_UpdateTokenById", true))
            {
                connector.AddInParameterWithValue("@UserId", userId);
                connector.AddInParameterWithValue("@Token", token);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return rowEffected > 0;
        }

        public bool UpdateToken(string mobilenumber, string emailaddres, string token, out long userid, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_UpdateTokenByNoEmail", true))
            {
                connector.AddInParameterWithValue("@mobileno", mobilenumber);
                connector.AddInParameterWithValue("@emailaddress", emailaddres);
                connector.AddInParameterWithValue("@Token", token);
                connector.AddOutParameterWithType("@UserId", SqlDbType.BigInt);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                userid= Convert.ToInt64(connector.GetParamaeterValue("@UserId").ToString());
            }

            return rowEffected > 0;
        }

        public bool UpdateMobileEmailVerification(string mobilenumber, string emailaddres, bool isMobile,int OTP, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_UpdateVerification", true))
            {
                connector.AddInParameterWithValue("@MobileNumber", mobilenumber);
                connector.AddInParameterWithValue("@emailaddres", emailaddres);
                connector.AddInParameterWithValue("@OTP", OTP);
                connector.AddInParameterWithValue("@IsMobile", isMobile); // if true means mobile verification otherwise email verification
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return rowEffected > 0;
        }

        public DataTable UserLogIn(string mobilenumber, string emailaddres,string password, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetUserDetailsByUserLogin", true))
            {
                connector.AddInParameterWithValue("@MobileNumber", mobilenumber);
                connector.AddInParameterWithValue("@emailaddres", emailaddres);
                connector.AddInParameterWithValue("@password", password);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                dt = connector.GetDataTable();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return dt;
        }

        public bool UpdateUserPassword(string mobilenumber, string emailaddres, string password, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_UpdatePassword", true))
            {
                connector.AddInParameterWithValue("@MobileNumber", mobilenumber);
                connector.AddInParameterWithValue("@emailaddres", emailaddres);
                connector.AddInParameterWithValue("@password", password);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return rowEffected > 0;
        }

        public DataTable ForgotUserLoginDetail(string mobilenumber, string emailaddres, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_ForgotUserLoginDetail", true))
            {
                connector.AddInParameterWithValue("@MobileNumber", mobilenumber);
                connector.AddInParameterWithValue("@emailaddres", emailaddres);                
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                dt = connector.GetDataTable();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return dt;
        }

        public DataTable GetTransportType()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetTransportType", true))
            { 
                dt = connector.GetDataTable();
            }

            return dt;
        }
    }
}
