using System;
using System.Collections.Generic;
using System.IO;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Models;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
using Fanex.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NegativeBalanceByMatch.Business.Entities;

namespace Fanex.NegativeBalanceByMatch.Business.Business
{
	public class MailService
	{
		private readonly ILicenseeService _licenseeService;
		private readonly NegativeBalanceByMatchService _negativeBalanceByMatchService;
		private INegativeBalanceByMatchExcelService _negativeBalanceByMatchExcelService;

		public MailService(
			ILicenseeService licenseeService,
			NegativeBalanceByMatchService negativeBalanceByMatchService,
			INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService)
		{
			_licenseeService = licenseeService;
			_negativeBalanceByMatchService = negativeBalanceByMatchService;
			_negativeBalanceByMatchExcelService = negativeBalanceByMatchExcelService;
		}

		public void SendMails()
		{
			IDictionary<string, IList<Match>> licenseeToMatchs = _negativeBalanceByMatchService.GetLicenseeMatchsMap();

			DateTime curDateTime = DateTime.UtcNow;
			ExportSettings exportSettings = new ExportSettings();

			foreach (LicenseeFunction licenseeFunction in _licenseeService.GetLicenseeFunctions())
			{
				string licenseeName = licenseeFunction.LicenseeName;

				if (licenseeToMatchs[licenseeName] != null &&
					licenseeToMatchs[licenseeName].Count > 0)
				{
					Mail mail = new Mail
					{
						Subject = "Mail Station Test (Function Mail).",
						Title = "Mail Station Test (Function Mail).",
						Body = "Email Body.",
						MailingListId = licenseeFunction.Function.Id
					};

					string fileNamePath = Path.Combine(AppSettings.ExportDataDirectory,
													licenseeName + "_" + curDateTime.ToString("yyyy-MM-dd") + ".xlsx");

					_negativeBalanceByMatchExcelService.GenerateReportForLicensee(licenseeName, licenseeToMatchs[licenseeName], exportSettings, fileNamePath);

					mail.Attachments = new List<MailAttachment>();
					mail.Attachments.Add(new MailAttachment(fileNamePath,
						MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

					//MailClientHandler.Start();
					//MailClientHandler.SendMail(mail);
					//MailClientHandler.Stop();
				}
			}
		}
	}
}
