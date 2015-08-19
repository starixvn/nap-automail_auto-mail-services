// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Service
{
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.SMAReport.Business.Service;
    using Fanex.Service.Core.MailService;    

    public static class Program
    {
        private static void Main()
        {
            DbSettingProviderManager.Current.Start();
            IMailingServiceBase service = new MailingService();
            var mail = service.SendMailToVIPGroup();
        }
    }
}