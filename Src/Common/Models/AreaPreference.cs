using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QTrans.Models
{
    public class AreaPreference
    {
        public long preferenceId { get; set; }
        public long UserId { get; set; }
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringAlphNumeric)]
        [DisplayName("Area Preference")]
        [StringLength(20)]
        public string Area { get; set; }

    }  
}
