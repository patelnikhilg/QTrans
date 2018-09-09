using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebAPI.Models
{
    public class ChangePassword
    {
        public string mobilenumber { get; set; }
        public string emailaddres { get; set; }
        public string password { get; set; }
    }
}