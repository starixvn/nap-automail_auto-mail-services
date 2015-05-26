// <copyright file="EmailHelper.cs" company="Nexcel">
// TODO: Mapping Application setting to class
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.Client.Enumerations;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Models;
using Fanex.NAPReport.RngCasino.Business.Configuration;
using Robot.Report.RngCasino.Configuration;
using Robot.Report.RngCasino.Entities;
using Robot.Report.RngCasino.Helper.Implement;
using MailAttachment = Fanex.MailDelivery.Client.Models.MailAttachment;
using MailLinkedResource = Fanex.MailDeliver.Model.MailLinkedResource;

namespace Fanex.NAPReport.RngCasino.Business.Helper.Implement
{
	/// <summary>
	/// Send mail extension.
	/// </summary>
	public class EmailHelper
	{
		public static bool SendMail(string subject, string title, string body, string fileNamePath)
		{
			try
			{
				Mail mail = new Mail
				{
					Subject = subject,
					Title = title,
					Body = body,
					MailingListId = RngCasinoSettings.FunctionId
				};

				if (RngCasinoSettings.AttachReport == true)
				{
					mail.Attachments = new List<MailAttachment>
					{
						new MailAttachment(fileNamePath, FileExtension.MS_EXCEL)
					};
				}

				MailClientHandler.Start();
				MailClientHandler.SendMail(mail);
				MailClientHandler.Stop();

				return true;
			}
			catch (Exception ex)
			{
				LogHelper.Log(ex);
				return false;
			}
		}

		/// <summary>
		/// Sends the mail.
		/// </summary>
		public static bool SendMail(string to, string cc, string bcc, string mailTitle, string subject, string body, Robot.Report.RngCasino.Entities.MailAttachment mailAttachment)
		{
			try
			{
				var mailSend = new MailSend()
					{
						To = to,
						Cc = cc,
						Bcc = bcc,
						Title = mailTitle,
						Subject = subject,
						Body = body,
						Attachment = mailAttachment,
						AltView = null
					};

				if (!EmailSetting.UseOutsideMail)
				{
					SendMailViaNexcelSystem(mailSend);
					return true;
				}

				SendMailViaOutsideSystem(mailSend);
				return true;
			}
			catch (Exception ex)
			{
				LogHelper.Log(ex);
			}

			return false;
		}

		/// <summary>
		/// Sends the mail via NEXCEL system.
		/// </summary>
		private static bool SendMailViaNexcelSystem(MailSend mailSend)
		{
			try
			{
				using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
				{
					var mailItem = new MailRecord
						{
							MailTo = mailSend.To,
							MailCc = mailSend.Cc,
							MailBcc = mailSend.Bcc,
							Title = mailSend.Title,
							Subject = mailSend.Subject,
							Body = mailSend.Body
						};

					if (mailSend.Attachment != null && mailSend.Attachment.FileName != null)
					{
						mailItem.Attachments = new List<Fanex.MailDeliver.Model.MailAttachment>();
						var attItem = new Fanex.MailDeliver.Model.MailAttachment
							{
								FileName = mailSend.Attachment.FileName,
								Content = mailSend.Attachment.Content,
								ContentType = GetAttachmentContentType(mailSend.Attachment.ContentType)
							};

						mailItem.Attachments.Add(attItem);
					}

					if (mailSend.AltView != null)
					{
						mailItem.AlternateView = new List<MailLinkedResource>();
						var linkedResource = mailSend.AltView.LinkedResources;
						foreach (var linked in linkedResource)
						{
							var stream = linked.ContentStream;
							byte[] buffer = new byte[stream.Length];
							stream.Read(buffer, 0, buffer.Length);
							var resource = new MailLinkedResource
								{
									FileName = string.Empty,
									ImageType = FileExtentions.IMG_GIF,
									ContentCID = linked.ContentId,
									StreamContent = new MemoryStream(buffer)
								};
							mailItem.AlternateView.Add(resource);
						}
					}

					client.SendMail(mailItem);
				}

				return true;
			}
			catch (Exception ex)
			{
				LogHelper.Log(ex);
				return false;
			}
		}

		/// <summary>
		/// Sends the mail via outside system.
		/// </summary>
		/// <param name="mailDetail">The mail detail.</param>
		/// <returns>Send mail success or not.</returns>
		private static bool SendMailViaOutsideSystem(MailSend mailDetail)
		{
			var msg = new MailMessage();
			msg.To.Add(mailDetail.To.Replace(";", ","));
			if (mailDetail.Cc.Length > 0)
			{
				msg.CC.Add(mailDetail.Cc.Replace(";", ","));
			}

			if (mailDetail.Bcc.Length > 0)
			{
				msg.CC.Add(mailDetail.Bcc.Replace(";", ","));
			}

			msg.From = new MailAddress(EmailSetting.From, EmailSetting.To, System.Text.Encoding.UTF8);
			msg.Subject = EmailSetting.Subject;
			msg.SubjectEncoding = System.Text.Encoding.UTF8;
			msg.IsBodyHtml = true;
			msg.Body = mailDetail.Body;
			msg.BodyEncoding = System.Text.Encoding.UTF8;

			if (mailDetail.Attachment.Content.Length > 0)
			{
				var attachFile = new Attachment(mailDetail.Attachment.Content, mailDetail.Attachment.FileName);
				msg.Attachments.Add(attachFile);
			}

			msg.Priority = MailPriority.High;

			var client = new SmtpClient();
			client.Credentials = new System.Net.NetworkCredential(EmailSetting.From, EmailSetting.Password);
			client.Port = EmailSetting.MailPort;
			client.Host = EmailSetting.MailHost;
			client.EnableSsl = true;
			try
			{
				client.Send(msg);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the type of the attachment content.
		/// </summary>
		/// <param name="attachContentType">Type of the attach content.</param>
		/// <returns>File extension: XLS, DOC...</returns>
		/// <exception cref="System.ArgumentNullException">File name cannot be null.</exception>
		private static FileExtentions GetAttachmentContentType(string attachContentType)
		{
			if (string.IsNullOrEmpty(attachContentType))
			{
				throw new ArgumentNullException("File name cannot be null");
			}

			if (attachContentType.ToLower().IndexOf("excel", System.StringComparison.Ordinal) != -1)
			{
				return FileExtentions.MS_EXCEL;
			}

			return attachContentType.ToLower().IndexOf("doc", System.StringComparison.Ordinal) != -1
					   ? FileExtentions.MS_DOC
					   : FileExtentions.IMG_GIF;
		}
	}
}
