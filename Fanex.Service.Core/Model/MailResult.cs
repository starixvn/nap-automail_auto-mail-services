// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.MailDelivery.Core.Models;

namespace Fanex.Service.Core.Model
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailResult
    {
        public MailResult()
        {
            Mail = new Mail();
            JobStatus = JobStatus.Success;
        }

        public Mail Mail { get; set; }

        public JobStatus JobStatus { get; set; }
    }
}