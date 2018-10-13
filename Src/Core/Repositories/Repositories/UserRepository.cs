using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Utility;
using System;
using System.Collections.Generic;

namespace QTrans.Repositories
{
    public class UserRepository
    {
        private UserDataAccess instanceUser;
        public UserRepository()
        {
            this.instanceUser = new UserDataAccess();
        }

        public ResponseSingleModel<UserProfile> DeviceRegistration(string mobileNo, out string message)
        {
            var result = new ResponseSingleModel<UserProfile>();
            message = string.Empty;
            UserProfile user = new UserProfile();
            user.mobilenumber = mobileNo;
            user.OTP = CommonFunction.GenerateOTP();
            long identity;
            if (instanceUser.InsertUpdateUserDetails(user, out identity, out message))
            {
                user.userid = identity;
                var cmp = new Company() { UserId = identity };
                new CompanyRepository(identity).CompanyRegistration(cmp, out message);
                result.Response = user;
                result.Status = Constants.WebApiStatusOk;
                result.Message = message;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }

            return result;
        }

        public ResponseSingleModel<bool> UserTypeRegistration(long userId, long companyId, int userType, out string message)
        {
            var result = new ResponseSingleModel<bool>();
            message = string.Empty;
            result.Response = instanceUser.InsertUserCompanyMapping(userId, companyId, userType, out message);
            result.Status = Constants.WebApiStatusOk;
            result.Message = message;
            return result;
        }


        public ResponseCollectionModel<TransportType> GetTransportType()
        {
            var result = new ResponseCollectionModel<TransportType>();
            var data = instanceUser.GetTransportType();
            result.Response = DataAccessUtility.ConvertToList<TransportType>(data);
            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        public ResponseCollectionModel<TransportType> GetTransportTypeByUserId(long userid, out string message)
        {
            var result = new ResponseCollectionModel<TransportType>();
            message = string.Empty;
            var data = instanceUser.GetTransportTypeByUserId(userid, out message);
            result.Response = DataAccessUtility.ConvertToList<TransportType>(data);
            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        public ResponseSingleModel<UserProfile> WebRegistration(UserProfile user, out string message)
        {
            var result = new ResponseSingleModel<UserProfile>();
            message = string.Empty;
            user.OTP = new Random().Next(10000, 999999);
            long userid;
            if (instanceUser.InsertUpdateUserDetails(user, out userid, out message))
            {
                var dt = instanceUser.GetById(userid, out message);
                var lst = DataAccessUtility.ConvertToList<UserProfile>(dt);
                result.Response = lst !=null && lst.Count > 0 ? lst[0] : null;
                result.Status = Constants.WebApiStatusOk;
                result.Message = message;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }

            return result;
        }

        public ResponseSingleModel<UserProfile> UpdateUserProfile(UserProfile user, out string message)
        {
            var result = new ResponseSingleModel<UserProfile>();
            message = string.Empty;
            long userid;
            if (instanceUser.InsertUpdateUserDetails(user, out userid, out message))
            {
                result.Response= user;
                result.Status = Constants.WebApiStatusOk;
                result.Message = message;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }
            return result;
        }

        public ResponseSingleModel<bool> ChangePassword(string mobileNo, string emailAddress, string oldPassword, string password, out string message)
        {
            var result = new ResponseSingleModel<bool>();
            message = string.Empty;
            result.Response= instanceUser.UpdateUserPassword(mobileNo, emailAddress, oldPassword, password, out message);
            result.Status = Constants.WebApiStatusOk;
            result.Message = message;
            return result;
        }

        public ResponseSingleModel<bool> VerificationMobileEmail(long userId, int OTP, bool isMobile, out string token, out string message)
        {
            var result = new ResponseSingleModel<bool>();
            message = string.Empty;
            token = string.Empty;
            if (instanceUser.UpdateMobileEmailVerification(userId, OTP, isMobile, out message))
            {
                token = EncryptDecryptHelper.ComputePasswordHash(userId.ToString(), userId.ToString());
                result.Response = instanceUser.UpdateToken(userId, token, out message);
                result.Status = Constants.WebApiStatusOk;
                result.Message = message;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }
            return result;
        }


        public ResponseSingleModel<bool> ForgotUserLoginDetail(string mobileNo, string emailAddress, out string message)
        {
            var result = new ResponseSingleModel<bool>();
            message = string.Empty;
            var dt = instanceUser.ForgotUserLoginDetail(mobileNo, emailAddress, out message);
            var lst = DataAccessUtility.ConvertToList<UserProfile>(dt);
            UserProfile user = lst != null &&  lst.Count > 0 ? lst[0] : null;
            result.Response = user != null;
            result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            if (user != null)
            {
                SmtpMailUtility.SendMail(user.emailaddress, "QTransSupport@gmail.com", "New Password for your QTrans account", "Hello, This is auto generated mail. Your password is :" + user.Password, false);
            }

            return result;
        }

        public ResponseSingleModel<UserProfile> GetUserDetailById(long userid, out string message)
        {
            var result = new ResponseSingleModel<UserProfile>();
            message = string.Empty;
            var dt = instanceUser.GetById(userid, out message);
            var lst = DataAccessUtility.ConvertToList<UserProfile>(dt);
            UserProfile user = lst.Count > 0 ? lst[0] : null;
            result.Response = user;
            result.Status = user != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }

        public ResponseSingleModel<UserProfile> GetUserDetailByToken(string token, out string message)
        {
            var result = new ResponseSingleModel<UserProfile>();
            message = string.Empty;
            var dt = instanceUser.GetBytoken(token, out message);
            var lst = DataAccessUtility.ConvertToList<UserProfile>(dt);
            UserProfile user = lst != null && lst.Count > 0 ? lst[0] : null;
            result.Response = user;
            result.Status = user != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }

        public ResponseSingleModel<UserProfile> Login(string username, string password, out string message)
        {
            var result = new ResponseSingleModel<UserProfile>();
            message = string.Empty;
            var dt = instanceUser.UserLogIn(username, password, out message);
            var lst = DataAccessUtility.ConvertToList<UserProfile>(dt);
            UserProfile user = lst != null && lst.Count > 0 ? lst[0] : null;
            result.Response = user;
            result.Status = user != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }

        public ResponseSingleModel<bool> UpdateMobileEmailVerification(string mobilenumber, string emailaddres, bool isMobile, int OTP, out long userid, out string token, out string message)
        {
            var result = new ResponseSingleModel<bool>();
            message = string.Empty;
            token = string.Empty;
            userid = 0;
            if (instanceUser.UpdateMobileEmailVerification(mobilenumber, emailaddres, isMobile, OTP, out message))
            {
                token = EncryptDecryptHelper.ComputePasswordHash(isMobile ? mobilenumber : emailaddres, isMobile ? mobilenumber : emailaddres);
                result.Response = instanceUser.UpdateToken(mobilenumber, emailaddres, token, out userid, out message);
            }
            
            result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;

            return result;
        }

        public ResponseSingleModel<int> UpdateUserPhoto(Int64 userID, string filePath, out string message)
        {
            try
            {
                var result = new ResponseSingleModel<int>();
                result.Response = instanceUser.UpdateUserPhoto(userID, filePath, out message);
                result.Status = result.Response > 0 ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
                result.Message = message;
                return result;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
