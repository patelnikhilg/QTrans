using QTrans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebPortal.Models.Posting
{
    public class PostingData
    {
        public PostingProfile profile { get; set; }

        public PostingDetails details { get; set; }
    }
}