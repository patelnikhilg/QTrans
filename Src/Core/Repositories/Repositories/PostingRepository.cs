using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ViewModel.Common;
using QTrans.Models.ViewModel.Posting;
using QTrans.Utility;
using System;
using System.Collections.Generic;
using System.Data;

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

        public PostingDetails PostingDetailCreation(PostingDetails postingDetails, out string message)
        {
            long dtlpostingid;
            message = string.Empty;
            if (this.instancePosting.InsertUpdatePostingDetails(postingDetails,out dtlpostingid, out message))
            {
                var ds = this.instancePosting.GetByPostingDetailsId(postingDetails.postingid, out message);
                var lstProfile = DataAccessUtility.ConvertToList<QTrans.Models.ViewModel.Posting.PostingProfileView>(ds.Tables[0]);
                var lstDetails = DataAccessUtility.ConvertToList<PostingDetails>(ds.Tables[1]);
                postingDetails = lstDetails.Count > 0 ? lstDetails[0] : null;
                postingDetails.postingProfile = lstProfile.Count > 0 ? lstProfile[0] : null;
                postingDetails.dtlpostingid = dtlpostingid;
            }

            return postingDetails;
        }

        public PostingDetails GetPostingDetailById(long postingId, out string message)
        {
            message = string.Empty;
            PostingDetails postingDetails = null;
            var ds = this.instancePosting.GetByPostingDetailsId(postingId, out message);
            var lstProfile = DataAccessUtility.ConvertToList<QTrans.Models.ViewModel.Posting.PostingProfileView>(ds.Tables[0]);
            var lstDetails = DataAccessUtility.ConvertToList<PostingDetails>(ds.Tables[1]);
            postingDetails = lstDetails.Count > 0 ? lstDetails[0] : null;
            postingDetails.postingProfile = lstProfile.Count > 0 ? lstProfile[0] : null;

            return postingDetails;
        }

        public PostingDetails GetPostingDetailByPostId(long postingId,out PostingProfile profile, out string message)
        {
            message = string.Empty;
            PostingDetails postingDetails = null;
            var ds = this.instancePosting.GetByPostingDetailsId(postingId, out message);
            var lstProfile = DataAccessUtility.ConvertToList<PostingProfile>(ds.Tables[0]);
            var lstDetails = DataAccessUtility.ConvertToList<PostingDetails>(ds.Tables[1]);
            profile= lstProfile.Count > 0 ? lstProfile[0] : null; 
            postingDetails = lstDetails.Count > 0 ? lstDetails[0] : null;
            return postingDetails;
        }

        public Models.ViewModel.Posting.PostingProfileView GetPostingProfileById(long postingId, out string message)
        {
            message = string.Empty;
            QTrans.Models.ViewModel.Posting.PostingProfileView postingProfile = null;
            var dt = this.instancePosting.GetById(postingId, out message);
            var lstProfile = DataAccessUtility.ConvertToList<QTrans.Models.ViewModel.Posting.PostingProfileView>(dt);
            postingProfile = lstProfile.Count > 0 ? lstProfile[0] : null;
            return postingProfile;
        }

        public List<PostingList> GetPostingListByUserId(long userId, bool isPast, out string message)
        {
            message = string.Empty;
            var dt = this.instancePosting.GetListPostingByUserId(userId, isPast, out message);
            var lstProfile = DataAccessUtility.ConvertToList<PostingList>(dt);

            return lstProfile.Count > 0 ? lstProfile : null;
        }

        public bool SubmitRatingByDtlPostId(long dtlpostId, long userId, Int16 rating, string comments, Int16 isRate)
        {
            return this.instancePosting.RatingByDtlPostUserId(dtlpostId, userId, rating, comments, isRate);
        }

        public List<Rating> PendingPostRatingByUserId(long userId)
        {
            var dt = this.instancePosting.PendingPostRatingByUserId(userId);
            return DataAccessUtility.ConvertToList<Rating>(dt);
        }

        public Dictionary<PostStatus,int> GetPostingStatsByUserId(long userId)
        {
            Dictionary<PostStatus, int> postStatus = new Dictionary<PostStatus, int>();
            postStatus.Add(Utility.PostStatus.None, 0);
            postStatus.Add(Utility.PostStatus.Open, 0);
            postStatus.Add(Utility.PostStatus.Close, 0);
            postStatus.Add(Utility.PostStatus.InProgress, 0);
            postStatus.Add(Utility.PostStatus.ConfirmPending, 0);
            postStatus.Add(Utility.PostStatus.ConfirmCompleted, 0);
            var dt = this.instancePosting.GetPostingStatsByUserId(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    switch (Convert.ToInt16(dr["PostStatus"].ToString()))
                    {
                        case 1:
                            postStatus[Utility.PostStatus.Open] = Convert.ToInt32(dr["Stats"].ToString());
                            break;
                        case 2:
                            postStatus[Utility.PostStatus.Close] = Convert.ToInt32(dr["Stats"].ToString());
                            break;
                        case 3:
                            postStatus[Utility.PostStatus.InProgress] = Convert.ToInt32(dr["Stats"].ToString());
                            break;
                        case 4:
                            postStatus[Utility.PostStatus.ConfirmPending] = Convert.ToInt32(dr["Stats"].ToString());
                            break;
                        case 5:
                            postStatus[Utility.PostStatus.ConfirmCompleted] = Convert.ToInt32(dr["Stats"].ToString());
                            break;
                        default:
                            postStatus[Utility.PostStatus.None] = Convert.ToInt32(dr["Stats"].ToString());
                            break;
                    }
                }
            }

            return postStatus;
        }

        public List<PostStatusList> GetPostingStatusByUserId(long userId, Int16 PostStatus)
        {
            var dt = this.instancePosting.GetPostingStatusByUserId(userId, PostStatus);
            return DataAccessUtility.ConvertToList<PostStatusList>(dt);
        }

        public bool UpdatePostingStatus(long dtlpostId,  Int16 PostStatus)
        {
            return this.instancePosting.UpdatePostingStatus(dtlpostId, PostStatus);
        }
    }
}
