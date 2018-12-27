using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.ViewModel.Posting
{
    public class PostingProfileView
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
        public DateTime createddate { get; set; }
        public DateTime? modifydate { get; set; }

        public long userid { get; set; }
    }
}
