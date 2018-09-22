using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebAPI.Models
{
    public class PostingParam
    {
        public long UserId { get; set; }

        public long PostingId { get; set; }

        public bool IsPast { get; set; } = false;
    }

}