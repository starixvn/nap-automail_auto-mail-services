// -----------------------------------------------------------------------
// <copyright file="MailAttachment.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Robot.Report.RngCasino.Entities
{
    using System.IO;

    /// <summary>
    /// Email's attachment detail.
    /// </summary>
    public class MailAttachment
    {
        public MemoryStream Content { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }
    }
}
