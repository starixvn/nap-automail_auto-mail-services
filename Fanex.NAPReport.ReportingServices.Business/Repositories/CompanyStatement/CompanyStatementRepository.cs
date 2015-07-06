// -----------------------------------------------------------------------
// <copyright file="CompanyStatementRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Repositories.CompanyStatement
{
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyStatement;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyStatement;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyStatementRepository : ICompanyStatementRepository
    {
        public CompanyStatement GetCompanyStatement(GetCompanyStatementCriteria criteria)
        {
            CompanyStatement companyStatement = new CompanyStatement();
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objectDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    companyStatement.ReportRows.Add(new CompanyStatement
                    {
                        Date = Converter.ToDateTime(reader["accdate"]).ToString("dddd M/d/yyyy"),
                        Type = Converter.ToString2(reader["transtype"]),
                        Total = Converter.ToDecimal(reader["totalamt"])
                    });
                }

                reader.Close();
            }

            return companyStatement;
        }
    }
}