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

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\CreditMovement\Settings\CreditMovement.Settings.xml");
            configurationParser.Parse<AppSettings>();

            var repository = new CreditMovementRepository();
            MailingService service = new MailingService();
            var mail = service.SendMailToVIPGroup();

            Console.WriteLine(string.Format("Done at {0}", DateTime.Now.ToString("yyyy-MM-dd")));
        }
    }
}