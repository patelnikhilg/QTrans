using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.Posting
{
    public class DtlPostingDetails
    {

        public DtlPostingDetails()
        {
        }

        public PostingProfile postingProfile { get; set; }
        public long dtlpostingid { get; set; }
        public long postingid { get; set; }

        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("Material Weight")]
        public int materialweight { get; set; }

        [DisplayName("Material Photo")]
        [StringLength(100)]
        public string materialphotos { get; set; }

        [DisplayName("Packing Dimension")]
        [StringLength(50)]
        public string packingdimension { get; set; }

        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("No of Packets")]
        public int numberpackets { get; set; }
        [DisplayName("Vehicle Type")]
        public string vehicletype { get; set; }

        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("No of Vehicle")]
        public short novehicle { get; set; }
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("Delivery Period in days")]
        public Nullable<short> deliveryperiodday { get; set; }

        [DisplayName("Pickup DateTime")]
        public System.DateTime pickupdatetime { get; set; }
        [RegularExpression(Constants.RegexDecimalInput, ErrorMessage = Constants.StringDecimal)]
        [DisplayName("Post Amount")]
        public Nullable<decimal> postamount { get; set; }

        [RegularExpression(Constants.RegexIntInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("On Pickup advance payment in percentage(%)")]
        public short onpickuppercentage { get; set; }

        [RegularExpression(Constants.RegexIntInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("On unload payment in percentage(%)")]
        public short onunloadingpercentage { get; set; }
        public int creditday { get; set; }

        [DisplayName("Contract Start DateTime")]
        public Nullable<System.DateTime> contractstartdatetime { get; set; }
        [DisplayName("Contract End DateTime")]
        public Nullable<System.DateTime> contractenddatetime { get; set; }

        [RegularExpression(Constants.RegexIntInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("Contract End DateTime")]
        public string ordertype { get; set; }

        [DisplayName("Bidding Activation DateTime")]
        public System.DateTime biddingactivatedatetime { get; set; }

        [DisplayName("Bidding Close DateTime")]
        public System.DateTime biddingclosedatetime { get; set; }

        [DisplayName("Status of Post")]
        public string poststatus { get; set; }

        [DisplayName("GPRS Enable")]
        public bool gprs { get; set; }

        [DisplayName("Men power of loading is required")]
        public bool menpowerloading { get; set; }
        [DisplayName("Men power of unloading is required")]
        public bool menpowerunloading { get; set; }

        [DisplayName("Transporter should provide insurcance")]
        public bool transporterinsurance { get; set; }
        [DisplayName("Toll Tax is include")]
        public bool tolltaxinclude { get; set; }

        [DisplayName("Remark")]
        [StringLength(200)]
        public string remark { get; set; }

        [DisplayName("Loading Type (Fullloading or Half loading)")]
        public bool loadingtype { get; set; }
        public Nullable<System.DateTime> createddate { get; set; }
        public Nullable<System.DateTime> modifydate { get; set; }
    }
}
