// -----------------------------------------------------------------------
// <copyright file="MailSend.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Robot.Report.RngCasino.Entities
{
    using System.Net.Mail;

    /// <summary>
    /// TODO: Email detail.
    /// </summary>
    public class MailSend
    {
        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public MailAttachment Attachment { get; set; }

        public AlternateView AltView { get; set; }
    }
}
