// -----------------------------------------------------------------------
// <copyright file="HorseRacingReport.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.NAPReport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.HorseRacingReport.Business;
    using Fanex.NAPReport.HorseRacingReport.Business.Criteria;
    using Fanex.NAPReport.HorseRacingReport.Business.Service;
    using Fanex.Service.Core.Extensions;
    using Quartz;
    using RazorEngine;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HorseRacingReport : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            IHorseRacingService service = new HorseRacingService();
            TimeRangeCriteria timeRangeCriteria = AssignRangeDate(service);

            service.GetData(timeRangeCriteria);
            string path = service.ReportZipFolderPath;

            var root = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = root + AppSettings.EmailTemplate;
            string template = File.ReadAllText(filePath);
            var emailContent = Razor.Parse(template, timeRangeCriteria);
            string emailSubject = string.Format(
                AppSettings.MailSubject,
                timeRangeCriteria.FromDate.ToShortFormatedDateWithSlash(),
                timeRangeCriteria.ToDate.ToShortFormatedDateWithSlash());

            Mail mail = new Mail()
            {
                Subject = emailSubject,
                Title = AppSettings.MailTitle,
                Body = emailContent
            };

            mail.Attachments = new List<MailAttachment>();
            mail.Attachments.Add(new MailAttachment(@service.ReportZipFolderPath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            
            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
                Result = new ResultMessage
                {
                    ErrCode = 0,
                    ErrMsg = string.Empty
                }
            };
        }

        private static TimeRangeCriteria AssignRangeDate(IHorseRacingService service)
        {
            TimeRangeCriteria timeRangeCriteria = new TimeRangeCriteria();

            bool isRunningDay = IsRunningDay();

            if (isRunningDay)
            {
                timeRangeCriteria = service.GetTimeRangeCriteria();
            }
            else
            {
                timeRangeCriteria.FromDate = DateTime.Today.AddDays(-4);
                timeRangeCriteria.ToDate = DateTime.Today.AddDays(-1);
            }

            return timeRangeCriteria;
        }

        private static bool IsRunningDay()
        {
            DateTime currentTime = DateTime.Now;
            return currentTime.DayOfWeek == DayOfWeek.Thursday || currentTime.DayOfWeek == DayOfWeek.Monday;
        }
    }
}
