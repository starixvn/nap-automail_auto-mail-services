// <copyright file="EmailHelper.cs" company="Nexcel">
// TODO: Mapping Application setting to class
// </copyright>

using System;
using System.Collections.Generic;
using Fanex.MailDelivery.Client.Enumerations;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Models;

namespace Fanex.NAPReport.RngCasino.Business.Helper.Implement
{
	/// <summary>
	/// Send mail extension.
	/// </summary>
	public class EmailHelper
	{
		public static bool SendMail(int functionId, string subject, string title, string body, string fileNamePath = null)
		{
            return false;
		}
	}
}
