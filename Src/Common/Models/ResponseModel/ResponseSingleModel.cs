﻿#region Copyright [CompanyName]
// ************************************************************************************
// <copyright file="ParseResultModel.cs" company="[CompanyName]">
// Copyright © 2018 [CompanyName]
// </copyright>
// ************************************************************************************
// <author></author>
// <project>Transport</project>
// ************************************************************************************
#endregion

namespace QTrans.Models.ResponseModel
{
    using System;

    /// <summary>
    /// This is a Response class. We used this class to send response from service layer to presentation layer.
    /// </summary>
    /// <typeparam name="T">Generic Type</typeparam>
    public class ResponseSingleModel<T>
    {
        /// <summary>
        ///  Gets or sets property. We will use it to assign response of user request.
        /// </summary>
        public T Response { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether status. We will use it to assign status of the response of user request.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets property. We will use it to assign exception which occurred during the execution of process.
        /// </summary>
        public Exception ErrorException { get; set; }
    }
}
