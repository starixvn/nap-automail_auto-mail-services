// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Service
{
    using System;
    using CreditMovement.Business.Service;
    using Fanex.CreditMovement.Business;
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.CreditMovement.Business.Repositories;
    using Fanex.Data.Configuration;
    using Fanex.Service.Core.Configuration;

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
            var service = new CreditMovementService();

            GetCreditMovementCriteria criteria = new GetCreditMovementCriteria
            {
                FromDate = AppSettings.FromDate,
                ToDate = AppSettings.ToDate,
                SiteList = string.Join(",", AppSettings.ListSite),
                Amount = AppSettings.Amount
            };

            service.ProcessToSendEmail(criteria, "Credit Movement Notification");

            Console.WriteLine(string.Format("Done at {0}", DateTime.Now.ToString("yyyy-MM-dd")));
        }
    }
}