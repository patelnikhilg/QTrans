using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QTrans.WebPortal.Models.Users
{
    public class ChangePassword
    {
        public long userId { get; set; }

        [DisplayName("Old Password")]
        [StringLength(10)]
        public string OldPassword { get; set; }
        [DisplayName("New Password")]
        [StringLength(10)]
        public string Newpassword { get; set; }
        [DisplayName("Confirm Password")]
        [StringLength(10)]
        public string ConfirmPassword { get; set; }
    }

    public class AreaPeferance
    {
        public long preferenceId { get; set; }
        public long UserId { get; set; }
        [DisplayName("Area Peferance")]
        public string Area { get; set; }
    }
}