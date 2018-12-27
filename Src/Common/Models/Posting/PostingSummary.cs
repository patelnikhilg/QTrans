using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.Posting
{
   public class PostingSummary
    {
        public int ActivePosts { get; set; }

        public int PastPosts { get; set; }

        public int ActvieBooking { get; set; }

        public int CompletedBooking { get; set; }

        public int CanceledBookingByYou { get; set; }

        public int CanceledBookingByVendor { get; set; }
        

    }
}
