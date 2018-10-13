﻿using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ViewModel.Common;
using QTrans.Models.ViewModel.Posting;
using System;
using System.Collections.Generic;

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
    }
}
