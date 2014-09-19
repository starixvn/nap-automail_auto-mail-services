// -----------------------------------------------------------------------
// <copyright file="ICompanyStatementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Franklin.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.ReportingService.Business.Service.Interface
{
    using Fanex.ReportingService.Business.Criteria.CompanyStatement;
    using Fanex.ReportingService.Business.Model.CompanyStatement;

    public interface ICompanyStatementService
    {
        CompanyStatementInfo GetCompanyStatement(GetCompanyStatementCriteria criteria);

        void ExportCompanyStatementReport(GetCompanyStatementCriteria criteria);
    }
}