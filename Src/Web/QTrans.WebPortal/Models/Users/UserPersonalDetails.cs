using QTrans.Models;
using QTrans.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

    public class UserCompany
    {
        public UserProfile userProfile { get; set; }

        public Company company { get; set; }

        public UserDetails userDetails { get; set; }

        public List<UserType> GetVendorType()
        {
            //Additng items to the list
            List<UserType> ChkItem = new List<UserType>
            {
                UserType.Transporter,
                UserType.TruckOwner,
                UserType.Broker
            };

            return ChkItem;
        }
    }
}