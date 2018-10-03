using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebAPI.Models
{
    public class BiddingParam
    {
        public long UserId { get; set; }

        public long DtlPostingId { get; set; }
    }

    public class BiddingUserParam
    {
        public long UserId { get; set; }

        public bool IsPast { get; set; }
    }
}