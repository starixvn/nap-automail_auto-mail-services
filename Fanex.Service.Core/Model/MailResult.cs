// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model
{
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;

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
