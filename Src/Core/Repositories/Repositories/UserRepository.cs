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
            long userid;
            if (instanceUser.InsertUpdateUserDetails(user, out userid, out message))
            {
                user.userid = userid;
                new CompanyRepository(userid).CompanyRegistration(new Company(), out message);
                return user;
            }

            return null;
        }

        public bool UserTypeRegistration(long userId, long companyId, int userType, out string message)
        {
            message = string.Empty;
            return instanceUser.InsertUserCompanyMapping(userId, companyId, userType, out message);
        }


        public List<TransportType > GetTransportType()
        {
            var data= instanceUser.GetTransportType();

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
    }
}
