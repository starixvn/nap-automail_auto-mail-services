// <copyright file="EmailHelper.cs" company="Nexcel">
// TODO: Mapping Application setting to class
// </copyright>

using System;
using System.Collections.Generic;
using Fanex.MailDelivery.Client.Enumerations;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Models;
using Fanex.NAPReport.RngCasino.Business.Configuration;

namespace Fanex.NAPReport.RngCasino.Business.Helper.Implement
{
	/// <summary>
	/// Send mail extension.
	/// </summary>
	public class EmailHelper
	{
		public static bool SendMail(int functionId, string subject, string title, string body, string fileNamePath = null)
		{
			try
			{
				var mail = new Mail
				{
					Subject = subject,
					Title = title,
					Body = body,
					MailingListId = functionId
				};

				if (fileNamePath != null)
				{
					if (RngCasinoSettings.AttachReport == true)
					{
						mail.Attachments = new List<MailAttachment>
						{
							new MailAttachment(fileNamePath, FileExtension.MS_EXCEL)
						};
					}
				}

				MailClientHandler.SendMail(mail);

				return true;
			}
			catch (Exception ex)
			{
				LogHelper.Log(ex);
				return false;
			}
		}
	}
}
