using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QTrans.WebPortal.Models.Login
{
    public class Login
    {
        [Requi]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        public string Password { get; set; }

    }

    public class Forgotpassword
    {
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }
    }
}