using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models
{
    public class Constants
    {
        public const string RegexStringInput = @"^[a-zA-Z0-9_\-\.\,\s\:]*$";

        public const string RegexStringPasswordInput = @"^[a-zA-Z0-9_\-\.\@\!]+$";

        public const string RegexIntInput = @"^[0-9]+$";

        public const string RegexMobileNoInput = @"^[0-9]{10}$";

        public const string RegexDecimalInput = @"^\d+(\.\d{1,2})?$";

        public const string StringAlphNumeric = "Only Alphabets and Numbers allowed. (A to Z small/capital letter and 0 to 9, undersore, dash and space)";

        public const string StringPwdAlphNumeric = "Only Alphabets and Numbers allowed. (A to Z small/capital letter and 0 to 9, _,-,.,@,! characters is allow)";

        public const string StringNumeric = "Only Numbers allowed.";

        public const string StringMobileNumber = "Mobile must be 10 digit";

        public const string StringDecimal = "Only Decimal allowed. e.g 12.50";
    }
}
