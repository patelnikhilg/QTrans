using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.ViewModel.Bidding
{
    public class PostingDetailsBid
    {
        public long postingid { get; set; }
        [DisplayName("Post Type")]
        public string posttype { get; set; }
        [DisplayName("Address")]
        public string soureaddress { get; set; }
        [DisplayName("LandMark")]
        public string src_landmark { get; set; }
        [DisplayName("City")]
        public string src_city { get; set; }
        [DisplayName("State")]
        public string src_state { get; set; }
        [DisplayName("Pincode")]
        public int src_pincode { get; set; }
        [DisplayName("Address")]
        public string destinationadress { get; set; }
        [DisplayName("LandMark")]
        public string dst_landmark { get; set; }
        [DisplayName("City")]
        public string dst_city { get; set; }
        [DisplayName("State")]
        public string dst_state { get; set; }
        [DisplayName("Pincode")]
        public int dst_pincode { get; set; }
        [DisplayName("Material Type")]
        public string materialtype { get; set; }
        [DisplayName("Material Description")]
        public string description { get; set; }
        [DisplayName("Package Type")]
        public string packagetype { get; set; }
        [DisplayName("Package Description")]
        public string packagetypedesc { get; set; }
        public long dtlpostingid { get; set; }

        [DisplayName("Material Weight")]
        public int materialweight { get; set; }

        [DisplayName("Material Photo")]
        [StringLength(100)]
        public string materialphotos { get; set; }

        [DisplayName("Packing Dimension")]
        [StringLength(50)]
        public string packingdimension { get; set; }

        [DisplayName("No of Packets")]
        public int numberpackets { get; set; }
        [DisplayName("Vehicle Type")]
        public short vehicletype { get; set; }

        [DisplayName("No of Vehicle")]
        public short novehicle { get; set; }
        [DisplayName("Delivery Period in days")]
        public Nullable<short> deliveryperiodday { get; set; }

        [DisplayName("Pickup Date")]
        public System.DateTime pickupdatetime { get; set; }
        [DisplayName("Post Amount")]
        public Nullable<decimal> postamount { get; set; }

        [DisplayName("On Pickup advance payment in percentage(%)")]
        public short onpickuppercentage { get; set; }

        [DisplayName("On unload payment in percentage(%)")]
        public short onunloadingpercentage { get; set; }
        public int creditday { get; set; }

        [DisplayName("Contract Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constants.FormatShortDate)]
        public Nullable<System.DateTime> contractstartdatetime { get; set; }
        [DisplayName("Contract End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constants.FormatShortDate)]
        public Nullable<System.DateTime> contractenddatetime { get; set; }

        [DisplayName("Order Type")]
        public short ordertype { get; set; }
        [DisplayName("Order Type")]
        public string ordertypeValue { get { return ((QTrans.Utility.OrderType)ordertype).ToString(); } }

        [DisplayName("Bidding Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constants.FormatShortDate)]
        public System.DateTime biddingactivatedatetime { get; set; }

        [DisplayName("Bidding Close Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constants.FormatShortDate)]
        public System.DateTime biddingclosedatetime { get; set; }

        [DisplayName("Status")]
        public short poststatus { get; set; }

        [DisplayName("Status")]
        public string poststatusvalue { get { return ((QTrans.Utility.PostStatus)ordertype).ToString(); } }

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

        [DisplayName("Last modify date")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = Constants.FormatDateTime)]
        public Nullable<System.DateTime> LastModifyDate { get { return modifydate == null ? createddate : modifydate; } }
        public Nullable<System.DateTime> modifydate { get; set; }

        public long postUserid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mobilenumber { get; set; }
        public decimal RatingAvg { get; set; }
        public int TotalRating { get; set; }
        public string photo { get; set; }

        public string city { get; set; }
        public string state { get; set; }
        public int TotalBids { get; set; }
        public decimal MinimumBid { get; set; }


        public List<PostingPhotos> PostingPhotoList { get; set; }

        public RatingByTransporter ratingByTransporter { get; set; }
    }

    public class RatingByTransporter
    {
        public long RatingId { get; set; }

        public Int16 Rating { get; set; }

        public Int16 Status { get; set; }

        public string Comments { get; set; }

        public DateTime CreateDate { get; set; }

        public long DtlpostingId { get; set; }

        public long UserId { get; set; }

        public long CreatedBy { get; set; }
    }


}
