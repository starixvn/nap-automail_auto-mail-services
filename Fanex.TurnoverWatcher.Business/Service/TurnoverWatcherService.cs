// -----------------------------------------------------------------------
// <copyright file="TurnoverWatcherService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TurnoverWatcher.Business.Service
{
    using System;
    using System.Configuration;
    using System.Linq;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;
    using Fanex.TurnoverWatcher.Business.Model;
    using Fanex.TurnoverWatcher.Business.Repositories;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TurnoverWatcherService : ITurnoverWatcherService
    {
        private ITurnoverWatcherRepository _repository;

        public TurnoverWatcherService()
        {
            _repository = new TurnoverWatcherRepository();
        }

        public int InsertTask(TurnoverWatcherTask task)
        {
            int id = _repository.InsertTask(task);
            return id;
        }

        /// <summary>
        /// Processes the turnover watcher.
        /// </summary>
        /// <exception cref="System.Exception">The exception.</exception>
        public void ProcessTurnoverWatcher()
        {
            TurnoverWatcherTask task = new TurnoverWatcherTask();
            bool sendMailSuccess = false;
            
            var turnoverInfo = _repository.GetTurnOverInfo()
                .OrderBy(x => x.CustomerInfo.PercentageUsed)
                .ToList();
            var sendMailLevels = turnoverInfo.Select(x => x.SendMailLevel).Distinct().ToList();

            for (var i = 0; i < sendMailLevels.Count; i++)
            {
                var fromPercentage = sendMailLevels[i];
                var toPercentage = i == sendMailLevels.Count - 1 ? 101 : sendMailLevels[i + 1];

                var notifiedCustomerMatchTurnovers = turnoverInfo.Where(
                    x => x.CustomerInfo.PercentageUsed >= fromPercentage
                    && (x.CustomerInfo.PercentageUsed < toPercentage ||
                        i == sendMailLevels.Count - 1)).ToList();
                if (notifiedCustomerMatchTurnovers != null && notifiedCustomerMatchTurnovers.Count > 0)
                {
                    _repository.InsertTask(task);
                    var mailReceiver = this.MailingReceiver();
                    sendMailSuccess = Mailing.SendTurnoverWatcherEmail(notifiedCustomerMatchTurnovers, fromPercentage, mailReceiver);
                    
                    task = _repository.GetCurrentTask();
                    task.EmailReceiver = string.Format(
                            "Email to: {0}; Email Cc: {1}; Email Bcc: {1}",
                            mailReceiver.To,
                            mailReceiver.Cc,
                            mailReceiver.Bcc);

                    if (sendMailSuccess)
                    {
                        _repository.UpdateTask(task, Status.Successful);
                        var resultRemove = _repository.RemoveNotifiedCustomer(notifiedCustomerMatchTurnovers, fromPercentage);
                        if (!resultRemove.ErrorCode.Equals(0))
                        {
                            Mailing.ErrorEmailNotification(fromPercentage, resultRemove);
                        }
                    }
                    else
                    {
                        _repository.UpdateTask(task, Status.Failed);
                    }
                }
            }
        }

        /// <summary>
        /// Mailings the receiver.
        /// </summary>
        /// <returns>The receiver.</returns>
        private EmailReceiver MailingReceiver()
        {
            int functionId = Convert.ToInt32(ConfigurationManager.AppSettings["functionid"]);
            var mailReceiver = new EmailReceiver();

            if (AppSettings.IsEmailConfig)
            {
                mailReceiver.To = ConfigurationManager.AppSettings["to"];
                mailReceiver.Cc = ConfigurationManager.AppSettings["cc"];
                mailReceiver.Bcc = ConfigurationManager.AppSettings["bcc"];

                return mailReceiver;
            }

            return _repository.MailingReceiver(functionId);
        }
    }
}