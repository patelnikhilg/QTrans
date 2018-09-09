using System;
using System.Collections.Generic;
using System.Text;
using QTrans.DataAccess;
using QTrans.Models;

namespace QTrans.Repositories
{
    public class PostingRepository
    {
        private PostingDataAccess instancePosting;
        private long UserId;
        public PostingRepository(long userid)
        {
            this.UserId = userid;
            this.instancePosting = new PostingDataAccess();
        }

        public PostingProfile PostingPorfileCreation(PostingProfile posting, out string message)
        {
            long postingId = 0;
            message = string.Empty;
            if (this.instancePosting.InsertUpdatePosting(posting, out postingId, out message))
            {
                posting.postingid = postingId; 
            }

            return posting;
        }

        public PostingDetails PostingDetailCreation(PostingDetails posting, out string message)
        {
            long postingId = 0;
             message = string.Empty;
            PostingDetails postingDetails = null;
            //if (this.instancePosting.InsertUpdatePosting(posting.postingProfile, out postingId, out message))
            //{
               // postingDetails.postingid = postingId;
                if(this.instancePosting.InsertUpdatePostingDetails(postingDetails, out message))
                {
                    var ds = this.instancePosting.GetByPostingDetailsId(postingId, out message);
                    var lstProfile = DataAccessUtility.ConvertToList<PostingProfile>(ds.Tables[0]);
                    var lstDetails = DataAccessUtility.ConvertToList<PostingDetails>(ds.Tables[1]);
                    postingDetails = lstDetails.Count > 0 ? lstDetails[0] : null;
                    postingDetails.postingProfile = lstProfile.Count > 0 ? lstProfile[0] : null;                    
                }
                
            //}

            return postingDetails;
        }

        public PostingDetails GetPostingDetailById(long postingId, out string message)
        {
             message = string.Empty;
            PostingDetails postingDetails = null;
            var ds = this.instancePosting.GetByPostingDetailsId(postingId, out message);
            var lstProfile = DataAccessUtility.ConvertToList<PostingProfile>(ds.Tables[0]);
            var lstDetails = DataAccessUtility.ConvertToList<PostingDetails>(ds.Tables[1]);
            postingDetails = lstDetails.Count > 0 ? lstDetails[0] : null;
            postingDetails.postingProfile = lstProfile.Count > 0 ? lstProfile[0] : null;

            return postingDetails;
        }

        public List<PostingDetails> GetPostingListByUserId(long userId, out string message)
        {
            message = string.Empty;
            return null;
        }
    }
}
