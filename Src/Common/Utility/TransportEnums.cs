using System;
using System.Collections.Generic;
using System.Text;

namespace QTrans.Utility
{
    //public class TransportEnums
    //{
    ////public enum VehicleType
    ////{
    ////    None = 0,
    ////    HalfBody = 1,
    ////    FullBody = 2,
    ////    OpenBody = 3,
    ////    Canter = 4,
    ////    CarCarrier = 5,
    ////    LCV = 6
    ////}

    public enum UserType
    {
        None = 0,
        Transporter = 1,
        TruckOwner = 2,
        Broker = 3,
        Customer = 4 //Supplier
    }

    public enum PaymentMethod
    {
        None = 0,
        NetBanking = 1,
        Debit_Credit_Card = 2,
        Paytm = 3
    }

    public enum PostStatus
    {
        None = 0,
        Open = 1,
        Close = 2,
    }

    public enum OrderType
    {
        None = 0,
        SingleParty = 1,
        Distributive = 2,
    }

    public enum BiddingStatus
    {
        None = 0,
        Confirm = 1,
        Reject = 2,
        Cancel = 3
    }
    //  }
}
