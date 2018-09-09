#region Copyright [CompanyName]
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
    /// <summary>
    /// The generic parse result.
    /// </summary>
    /// <typeparam name="T">Generic Type</typeparam>
    public class ParseResultModel<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether success.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public T Result { get; set; }
    }
}
