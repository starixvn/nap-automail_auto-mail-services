// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Service
{
    using System.ServiceProcess;
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.Data.Configuration;
    using System;
    using CreditMovement.Business.Service;
    using Fanex.CreditMovement.Business.Repositories;
    using Fanex.CreditMovement.Business;

    /// <summary>
    /// The main program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            DbSettingProviderManager.Current.Start();
            var repository = new CreditMovementRepository();
            GetCreditMovementCriteria criteria = new GetCreditMovementCriteria
            {
                FromDate = AppSettings.FromDate,
                ToDate = AppSettings.ToDate,
                SiteList = string.Join(",", AppSettings.ListSite),
                Amount = AppSettings.Amount
            };
            var service = new CreditMovementService();
            service.ProcessToSendEmail(criteria);

            Console.WriteLine(string.Format("Done at {0}", DateTime.Now.ToString("yyyy-MM-dd")));
        }
    }
}