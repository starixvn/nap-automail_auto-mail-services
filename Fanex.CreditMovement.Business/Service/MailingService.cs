// -----------------------------------------------------------------------
// <copyright file="MailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Service
{
    using System;
    using System.Collections.Generic;
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingService : IMailingServiceBase
    {
        private IMailStationService _mailStation;
        private MailInfo _mailInfo;

        private ICreditMovementService _excel;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\CreditMovement\Settings\CreditMovement.Settings.xml");
            configurationParser.Parse<AppSettings>();

            _mailStation = new MailStationService();
            _mailInfo = new MailInfo();

            _excel = new CreditMovementService();
        }

        public MailResult SendMailToVIPGroup()
        {
            MailResult mailResult = new MailResult();
            try
            {
                GetCreditMovementCriteria criteria = new GetCreditMovementCriteria
                {
                    FromDate = AppSettings.FromDate,
                    ToDate = AppSettings.ToDate,
                    SiteList = string.Join(",", AppSettings.ListSite),
                    Amount = AppSettings.Amount
                };

                _excel.ExportExcel(criteria);
                
                if (string.IsNullOrEmpty(_excel.FileName))
                {
                    mailResult.JobStatus = MailDelivery.Client.Enumerations.JobStatus.SuccessWithoutSendmail;
                }

                var fileNamesPath = new List<string>();
                fileNamesPath.Add(_excel.DirectoryOfFileName);

                _mailInfo = new MailInfo()
                {
                    FileName = _excel.FileName,
                    FileNamesPath = fileNamesPath
                };

                var mail = _mailStation.CreateMailWithAttachedFile(_mailInfo);
                mailResult.Mail = mail;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return mailResult;
        }

        public void SendMailToNormalGroup()
        {
            if (AppSettings.FunctionId > 0)
            {
                _mailStation.SendFunctionMailWithNotification(AppSettings.FunctionId, _mailInfo);
            }
        }
    }
}
