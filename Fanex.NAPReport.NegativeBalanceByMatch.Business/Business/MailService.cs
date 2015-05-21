using System;
using System.Collections.Generic;
using System.IO;
using Fanex.MailDelivery.Client.Enumerations;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Models;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Configuration;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business
{
	public class MailService
	{
		private readonly INegativeBalanceByMatchExcelService _negativeBalanceByMatchExcelService;

		public MailService(INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService)
		{
			_negativeBalanceByMatchExcelService = negativeBalanceByMatchExcelService;
		}

		public void SendMail(LicenseeFunction licenseeFunction, string subject, string title, string body, bool reportAttached,
			IList<Match> matches, DateTime dateTime, ExportSettings exportSettings)
		{
			string licenseeName = licenseeFunction.LicenseeName;

			Mail mail = new Mail
			{
				Subject = subject,
				Title = title,
				Body = body,
				MailingListId = licenseeFunction.Function.Id
			};

			string fileNamePath = Path.Combine(Settings.ExportDataDirectoryWithYearMonth,
									licenseeName + "_" + dateTime.ToString("yyyy-MM-dd-hh-mm-ss") + ".xlsx");

			_negativeBalanceByMatchExcelService.GenerateReportForLicensee(licenseeName, matches, exportSettings, fileNamePath);

			if (reportAttached == true)
			{
				mail.Attachments = new List<MailAttachment>
                {
                    new MailAttachment(fileNamePath, FileExtension.MS_EXCEL)
                };
			}

			MailClientHandler.Start();
			MailClientHandler.SendMail(mail);
			MailClientHandler.Stop();
		}
	}
}
