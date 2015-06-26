// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Service
{
    using System.ServiceProcess;
    using System;
    using Fanex.Data.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.NAPReport.SMAReport.Business.Service;    
    

    /// <summary>
    /// Main program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            DbSettingProviderManager.Current.Start();
            IMailingServiceBase service = new MailingService();
            var mail = service.SendMailToVIPGroup();
        }
    }
}