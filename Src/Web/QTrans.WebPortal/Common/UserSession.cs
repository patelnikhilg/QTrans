using QTrans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebPortal.Common
{
    public class UserSession
    {
        public long UserId { get; set; }

        public string LoginUserName { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNo { get; set; }

        public DateTime LastLogin { get; set; }

        public string Token { get; set; }

        public void SetValue(UserProfile user)
        {
            this.UserId = user.userid;
            this.LoginUserName = string.Concat(user.firstname, " ", user.lastname);
            this.EmailAddress = user.emailaddress;
            this.FirstName = user.firstname;
            this.LastName = user.lastname;
            this.MobileNo = user.mobilenumber;
            this.LastLogin = user.modifydate ?? user.createddate;
        }
    }
}