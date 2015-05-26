// -----------------------------------------------------------------------
// <copyright file="MailAttachment.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;

namespace Fanex.NAPReport.RngCasino.Business.Entities
{
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
