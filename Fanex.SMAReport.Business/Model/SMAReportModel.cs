// -----------------------------------------------------------------------
// <copyright file="SMAReportModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.SMAReport.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Service.Core.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAReportModel
    {
        public int Id { get; set; }

        public Status TaskStatus { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        public int TaskType { get; set; }
    }
}
