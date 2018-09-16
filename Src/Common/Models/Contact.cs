using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models
{
    public class Contact
    {
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringAlphNumeric)]
        [DisplayName("Name")]
        [Required(ErrorMessage = "The Name is required")]
        [StringLength(30)]
        public string Name { get; set; }

        [RegularExpression(Constants.RegexMobileNoInput, ErrorMessage = Constants.StringMobileNumber)]
        [DisplayName("Mobile No.")]
        [Required(ErrorMessage = "The mobile number is required")]
        [StringLength(12)]
        public string MobileNo { get; set; }


        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Email Address")]
        [StringLength(50)]
        public string emailaddress { get; set; }


        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringAlphNumeric)]
        [DisplayName("State")]
        [Required(ErrorMessage = "The state is required")]
        [StringLength(20)]
        public string state { get; set; }


        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringAlphNumeric)]
        [DisplayName("City")]
        [Required(ErrorMessage = "The city is required")]
        [StringLength(20)]
        public string city { get; set; }

        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringAlphNumeric)]
        [DisplayName("Message")]
        [Required(ErrorMessage = "The Message is required")]
        [StringLength(400)]
        public string Message { get; set; }

    }
}
