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

    public  class Company
    {
        public Company()
        {
        }
    
        public long companyid { get; set; }
        
        public string companyname { get; set; }
        
        public string address { get; set; }
        
        public string telenumber { get; set; }
        public string alternettelnumber { get; set; }
        
        public string country { get; set; }
        
        public string state { get; set; }
        
        public string city { get; set; }
        public long userid { get; set; }
        
        public int comanytype { get; set; } 
        public System.DateTime createddate { get; set; }
        public Nullable<System.DateTime> modifydate { get; set; }
    }
}