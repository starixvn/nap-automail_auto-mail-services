﻿namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.BA
 {
     using System;
     using System.Collections.Generic;
     using System.IO;
     using Fanex.NAPReport.BADailyWinLoss.Business;
     using Fanex.NAPReport.BADailyWinLoss.Business.Resource;
     using Fanex.NAPReport.BADailyWinLoss.Business.Service;
     using Fanex.MailDelivery.Client.Enumerations;
     using Fanex.MailDelivery.Client.Jobs;
     using Fanex.MailDelivery.Client.Messages;
     using Fanex.MailDelivery.Client.Models;
     using Quartz;

     /// <summary>
     /// TODO: Update summary.
     /// </summary>
     public class DailyWinLoss : JobBase
     {
         protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
         {
             var isTesting = AppSettings.IsTesting;
             var testedDate = AppSettings.TestedDate;
             var winLossDate = isTesting ? testedDate : DateTime.Now.AddDays(-1);

             var service = new BAReportService();
             service.GenerateReport(winLossDate);

             string filePath = AppDomain.CurrentDomain.BaseDirectory + AppSettings.TemplateDirectory;
             string template = File.ReadAllText(filePath);
             var emailContent = string.Format(template, winLossDate.ToShortDateString());

             var mail = new Mail()
             {
                 Subject = string.Format(Resource.Subject, winLossDate.ToShortDateString()),
                 Title = Resource.Title,
                 Body = emailContent
             };
             mail.Attachments = new List<MailAttachment>();
             mail.Attachments.Add(new MailAttachment(@service.DirectoryOfFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

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
     }
 }