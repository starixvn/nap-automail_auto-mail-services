// -----------------------------------------------------------------------
// <copyright file="GetCompanyStatementCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyStatement
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetCompanyStatementCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "fromdate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "todate")]
        public DateTime ToDate
        {
            get
            {
                return this.FromDate;
            }
        }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Statement_Get_Company_Statement";
        }
    }
}