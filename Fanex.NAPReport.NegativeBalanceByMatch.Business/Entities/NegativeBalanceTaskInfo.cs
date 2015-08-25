// -----------------------------------------------------------------------
// <copyright file="NegativeBalanceTaskInfo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities
{
    using Fanex.Service.Core.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NegativeBalanceTaskInfo
    {
        public int Id { get; set; }

        public Status TaskStatus { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }
    }
}