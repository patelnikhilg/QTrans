//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QTrans.WebPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public  class UserProfile
    {
        public UserProfile()
        {
           // this.areaPreferences =  new HashSet<AreaPreference>();
        }
    
        public long userid { get; set; }
        
        public string emailaddress { get; set; }
        
        public string firstname { get; set; }
        
        public string middlename { get; set; }
        
        public string lastname { get; set; }
        public string mobilenumber { get; set; }
        
        public string landlinenumber { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        
        public string addressline1 { get; set; }
        
        public string addressline2 { get; set; }
        
        public int pincode { get; set; }
        
        public string photo { get; set; }
        
        public string country { get; set; }
        
        public string state { get; set; }
        
        public string district { get; set; }
        
        public string city { get; set; }
        
        public string area { get; set; }

        public bool mobileverification { get; set; }
        public bool emailverification { get; set; }
        
        public string pan { get; set; }
        
        public string gst { get; set; }
        public Nullable<long> aadhaarno { get; set; }
        public DateTime createddate { get; set; }
        public Nullable<DateTime> modifydate { get; set; }

        public int OTP { get; set; }

        public string Token { get; set; }

        //public ICollection<AreaPreference> areaPreferences { get; set; }
    }
}