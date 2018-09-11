using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QTrans.WebPortal.Models.Login
{
    public class Login
    {        
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }

    }

    public class Forgotpassword
    {
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
        [DisplayName("Mobile No.")]
        public string MobileNo { get; set; }
    }
}
