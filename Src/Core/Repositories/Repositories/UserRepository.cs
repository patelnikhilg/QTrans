using QTrans.DataAccess;
using QTrans.Models;
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

        public UserProfile DeviceRegistration(string mobileNo, out string message)
        {
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
                return user;
            }

            return null;
        }

        public bool UserTypeRegistration(long userId, long companyId, int userType, out string message)
        {
            message = string.Empty;
            return instanceUser.InsertUserCompanyMapping(userId, companyId, userType, out message);
        }


        public List<TransportType> GetTransportType()
        {
            var data = instanceUser.GetTransportType();

            return DataAccessUtility.ConvertToList<TransportType>(data);

        }
        public UserProfile WebRegistration(UserProfile user, out string message)
        {
            message = string.Empty;
            UserProfile userDetail = null;
            user.OTP = new Random().Next(10000, 999999);
            long userid;
            if (instanceUser.InsertUpdateUserDetails(user, out userid, out message))
            {
                var dt = instanceUser.GetById(userid, out message);
                var lst = DataAccessUtility.ConvertToList<UserProfile>(dt);
                userDetail = lst.Count > 0 ? lst[0] : null;
            }

            return userDetail;
        }
               
        public UserProfile UpdateUserProfile(UserProfile user, out string message)
        {
            message = string.Empty;           
            long userid;
            if (instanceUser.InsertUpdateUserDetails(user, out userid, out message))
            {
                return user;
            }

            return null;
        }

        public bool ChangePassword(string mobileNo, string emailAddress, string password, out string message)
        {
            message = string.Empty;
            return instanceUser.UpdateUserPassword(mobileNo, emailAddress, password, out message);
        }

        public bool VerificationMobileEmail(long userId, int OTP, bool isMobile, out string token, out string message)
        {
            message = string.Empty;
            token = string.Empty;
            if (instanceUser.UpdateMobileEmailVerification(userId, OTP, isMobile, out message))
            {
                token = EncryptDecryptHelper.ComputePasswordHash(userId.ToString(), userId.ToString());
                return instanceUser.UpdateToken(userId, token, out message);
            }

            return false;
        }


        public UserProfile ForgotUserLoginDetail(string mobileNo, string emailAddress, out string message)
        {
            message = string.Empty;
            var dt = instanceUser.ForgotUserLoginDetail(mobileNo, emailAddress, out message);
            var lst = DataAccessUtility.ConvertToList<UserProfile>(dt);
            UserProfile user = lst.Count > 0 ? lst[0] : null;
            if (user != null)
            {
                SmtpMailUtility.SendMail(user.emailaddress, "QTransSupport@gmail.com", "New Password for your QTrans account", "Hello, This is auto generated mail. Your password is :" + user.Password, false);
            }
            return user;
        }

        public UserProfile GetUserDetailById(long userid, out string message)
        {
            message = string.Empty;
            var dt = instanceUser.GetById(userid, out message);
            var lst = DataAccessUtility.ConvertToList<UserProfile>(dt);
            UserProfile user = lst.Count > 0 ? lst[0] : null;
            return user;
        }

        public UserProfile GetUserDetailByToken(string token, out string message)
        {
            message = string.Empty;
            var dt = instanceUser.GetBytoken(token, out message);
            var lst = DataAccessUtility.ConvertToList<UserProfile>(dt);
            UserProfile user = lst.Count > 0 ? lst[0] : null;
            return user;
        }

        public UserProfile Login(string username,string password, out string message)
        {
            message = string.Empty;
            var dt = instanceUser.UserLogIn(username, password, out message);
            var lst = DataAccessUtility.ConvertToList<UserProfile>(dt);
            UserProfile user = lst.Count > 0 ? lst[0] : null;
            return user;
        }

        public bool UpdateMobileEmailVerification(string mobilenumber, string emailaddres, bool isMobile, int OTP, out long userid, out string token, out string message)
        {
            message = string.Empty;
            token = string.Empty;
            userid = 0;
            if (instanceUser.UpdateMobileEmailVerification(mobilenumber, emailaddres, isMobile, OTP, out message))
            {
                token = EncryptDecryptHelper.ComputePasswordHash(isMobile ? mobilenumber : emailaddres, isMobile ? mobilenumber : emailaddres);
                return instanceUser.UpdateToken(mobilenumber, emailaddres, token, out userid, out message);
            }

            return false;
        }
    }
}
