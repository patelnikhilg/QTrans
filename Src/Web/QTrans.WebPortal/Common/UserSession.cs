using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebPortal.Common
{
    public class UserSession
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNo { get; set; }

        public DateTime LastLogin { get; set; }

        public string Token { get; set; }
    }
}