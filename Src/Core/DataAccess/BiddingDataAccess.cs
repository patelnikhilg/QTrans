using QTrans.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace QTrans.DataAccess
{
    public class BiddingDataAccess
    {
        public bool InsertUpdateBiddingDetails(BiddingProfile bidding, out long identity, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertBidding", true))
            {
                List<BiddinguserType> userType = new List<BiddinguserType>();
                foreach (var details in bidding.biddingDetails)
                {
                    userType.Add(new BiddinguserType() { vehicleno = details.vehicleno, capacity = details.capacity });
                }
                connector.AddInParameterWithValue("@dtlbiddingid", bidding.biddingid);
                connector.AddInParameterWithValue("@dtlpostingid", bidding.dtlpostingid);
                connector.AddInParameterWithValue("@userid", bidding.userid);
                connector.AddInParameterWithValue("@amount", bidding.amount);
                connector.AddInParameterWithValue("@biddercomment", bidding.biddercomment);
                connector.AddInParameterWithValue("@status", bidding.status);
                connector.AddInParameterWithValue("@servicecharges", bidding.servicecharges);
                connector.AddInParameterWithValue("@paymentmethod", bidding.paymentmethod);
                connector.AddInParameterWithValue("@rating", bidding.rating);
                connector.AddInParameterWithValue("@cancellationreason", bidding.cancellationreason);
                connector.AddInParameterWithValue("@biddingDetails", DataAccessUtility.ToDataTable<BiddinguserType>(userType));
                connector.AddOutParameterWithType("@identity", SqlDbType.BigInt);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = bidding.biddingid == 0 ? Convert.ToInt64(connector.GetParamaeterValue("@identity")) : bidding.biddingid;
            }

            return rowEffected > 0;
        }

        public DataTable GetById(long biddingId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetBiddingDetailsById", true))
            {
                connector.AddInParameterWithValue("@biddingId", biddingId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetPostingDetailsByDtlPostingId(long dtlpostingId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetBiddingDetailsByDtlPostingId", true))
            {
                connector.AddInParameterWithValue("@DtlpostingId", dtlpostingId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetByUserId(long userId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetBiddingDetailsByUserId", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetPostingList(long userId, bool isPast)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostingList", true))
            {
                connector.AddInParameterWithValue("@UserId", userId);
                connector.AddInParameterWithValue("@isPast", isPast);
                dt = connector.GetDataTable();
            }

            return dt;
        }
    }
}
