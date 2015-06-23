// -----------------------------------------------------------------------
// <copyright file="MailInfo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailInfo
    {
        public MailInfo()
        {
            FileName = string.Empty;
            NapReportURL = AppSettingBase.NapReportURL;
            FileNamesPath = new List<string>();
        }

        public string FileName { get; set; }

        public string NapReportURL { get; set; }

        public List<string> FileNamesPath { get; set; }        
    }
}
