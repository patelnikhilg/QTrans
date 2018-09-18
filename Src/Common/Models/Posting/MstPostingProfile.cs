using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.Posting
{
    public class MstPostingProfile
    {
        public MstPostingProfile()
        {
        }

        public long postingid { get; set; }
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("Post Type")]
        [StringLength(10)]
        public string posttype { get; set; }
        public long userid { get; set; }
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("Source Address")]
        [StringLength(200)]
        public string soureaddress { get; set; }
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("Destination Address")]
        [StringLength(200)]
        public string destinationadress { get; set; }

        [RegularExpression(Constants.RegexIntInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("Material Type")]
        public string materialtype { get; set; }
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringAlphNumeric)]
        [DisplayName("Description")]
        [StringLength(200)]
        public string description { get; set; }

        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("Package Type")]
        [StringLength(200)]
        public string packagetype { get; set; }
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringAlphNumeric)]
        [DisplayName("Package Description")]
        [StringLength(200)]
        public string packagetypedesc { get; set; }
        public System.DateTime createddate { get; set; }
        public Nullable<System.DateTime> modifydate { get; set; }
    }
}
