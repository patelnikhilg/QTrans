using QTrans.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QTrans.WebPortal.Models.Login
{
    public class UserLogin
    {
        [Required(ErrorMessage = "User Name is required")]
        [StringLength(50)]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(10)]
        [DisplayName("Password")]
        public string Password { get; set; }

    }

    public class Forgotpassword
    {
        [StringLength(50)]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
        [DisplayName("Mobile No.")]
        [StringLength(12)]
        public string MobileNo { get; set; }
    }

   
}
