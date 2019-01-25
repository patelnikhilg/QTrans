using adminlte.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace adminlte.Models.Login
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Mobile Number is required")]
        [StringLength(10)]
        [DisplayName("Mobile Number")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(10)]
        [DisplayName("Password")]
        public string Password { get; set; }


        [DisplayName("Remember Me")]
        public bool remember { get; set; }
    }

    public class Forgotpassword
    {
        
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Mobile number is required")]
        [DisplayName("Mobile Number")]
        [StringLength(10)]
        public string MobileNo { get; set; }
    }

}
