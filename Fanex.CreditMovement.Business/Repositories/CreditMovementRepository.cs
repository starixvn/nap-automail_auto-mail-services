// -----------------------------------------------------------------------
// <copyright file="CreditMovementRepository.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.Common;
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.CreditMovement.Business.Model;
    using Fanex.Data;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// Credit movement repository.
    /// </summary>
    public class CreditMovementRepository : ICreditMovementRepository
    {
        /// <summary>
        /// Gets the report data.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Credit movement report data.</returns>
        public List<CreditMovement> GetReportData(GetCreditMovementCriteria criteria)
        {
            List<CreditMovement> report = new List<CreditMovement>();
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                IDataReader reader = objectDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    string downLineInformation = string.Empty;
                    string superName = Converter.ToString2(reader["SuperName"]);
                    downLineInformation = string.IsNullOrEmpty(superName) ? downLineInformation : superName;
                    string masterName = Converter.ToString2(reader["MasterName"]);
                    downLineInformation = string.IsNullOrEmpty(masterName) ? downLineInformation : string.Format("{0} / {1}", downLineInformation, masterName);
                    string agentName = Converter.ToString2(reader["AgentName"]);
                    downLineInformation = string.IsNullOrEmpty(agentName) ? downLineInformation : string.Format("{0} / {1}", downLineInformation, agentName);
                    string memberName = Converter.ToString2(reader["CustomerName"]);
                    downLineInformation = string.IsNullOrEmpty(memberName) ? downLineInformation : string.Format("{0} / {1}", downLineInformation, memberName);
                    report.Add(new CreditMovement()
                    {
                        Date = Converter.ToDateTime(reader["TransDate"]).ToShortFormatedDate(),
                        Currency = Converter.ToString2(reader["Currency"]),
                        Site = Converter.ToString2(reader["Site"]),
                        DownLinesInformation = downLineInformation,
                        OriginalCurrency = Converter.ToDecimal(reader["Amount"]),
                        BaseCurrency = Converter.ToDecimal(reader["BaseAmount"])
                    });
                }

                reader.Close();
            }

            return report;
        }
   
        /// <summary>
        /// Gets the site identifier.
        /// </summary>
        /// <returns>The list site identifier.</returns>
        public List<string> GetSiteId()
        {
            List<string> sites = new List<string>();
            using (ObjectDb objectDb = new ObjectDb("Misc_CreditMovement_GetSites"))
            {
                IDataReader reader = objectDb.ExecuteReader();
                while (reader.Read())
                {
                    sites.Add(Converter.ToString2(reader["siteid"]));
                }

                reader.Close();
            }

            return sites;
        }
    }
}