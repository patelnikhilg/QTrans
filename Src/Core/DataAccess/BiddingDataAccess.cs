using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTrans.Models;

namespace QTrans.DataAccess
{
    public class BiddingDataAccess
    {
        public bool InsertUpdateBiddingDetails(BiddingProfile bidding, out long identity, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertBidding", true))
            {
                connector.AddInParameterWithValue("@BiddingId", bidding.biddingid);
                connector.AddInParameterWithValue("@PostingId", bidding.postingid);
                connector.AddInParameterWithValue("@UserId", bidding.userid);
                connector.AddInParameterWithValue("@Amount", bidding.amount);
                connector.AddInParameterWithValue("@BidderComment", bidding.biddercomment);
                connector.AddInParameterWithValue("@Status", bidding.status);
                connector.AddInParameterWithValue("@ServiceCharges", bidding.servicecharges);
                connector.AddInParameterWithValue("@PaymentMethod", bidding.paymentmethod);
                connector.AddInParameterWithValue("@Rating", bidding.rating);
                connector.AddInParameterWithValue("@CancellationReson", bidding.cancellationreson);
                connector.AddInParameterWithValue("@biggingDetails", DataAccessUtility.ToDataTable<BiddingDetails>(bidding.biddingDetails.ToList()));
                connector.AddOutParameterWithType("@identity", SqlDbType.BigInt);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);              
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = Convert.ToInt64(connector.GetParamaeterValue("@identity"));
            }

            return rowEffected > 0;
        }

        public DataTable GetById(long biddingId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetBiddingDetailsById", true))
            {
                connector.AddInParameterWithValue("@biddingId", biddingId);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                dt = connector.GetDataTable();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return dt;
        }        
    }
}
