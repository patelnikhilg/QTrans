using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebPortal.Models.Users
{
    public class ChangePassword
    {
        public long userId { get; set; }
        public string OldPassword { get; set; }
        public string Newpassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class AreaPeferance
    {
        public long preferenceId { get; set; }
        public long UserId { get; set; }
        public string Area { get; set; }
    }
}