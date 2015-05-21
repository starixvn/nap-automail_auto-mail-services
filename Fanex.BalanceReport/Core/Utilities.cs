namespace Fanex.NAPReport.BalanceReport.Core
{
    using System;
    using System.Globalization;
    using System.IO;
    using Fanex.NAPReport.BalanceReport.Model;
    using System.Configuration;
    using RazorEngine;
    using Fanex.BalanceReport;

    /// <summary>
    /// Common class.
    /// </summary>
    public static class Utilities
    {
        public static string CreateDirectory(string DirectoryName)
        {
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory + "Data";
            var directoryInfo = new DirectoryInfo(rootDirectory);
            if (!directoryInfo.Exists) directoryInfo.Create();

            var _saveDirectoryCreditTerm = rootDirectory + "\\BalanceReport\\" + DirectoryName;
            directoryInfo = new DirectoryInfo(_saveDirectoryCreditTerm);
            if (!directoryInfo.Exists) directoryInfo.Create();
            return directoryInfo.FullName;
        }

        public static string LoadSubject(Report report)
        {
            return ParseTemplate("Subject.cshtml", report);
        }

        public static string LoadBody(Report report)
        {
            var template = report.IsDirectReport ? "Direct Balance Email.cshtml" : "Email.cshtml";
            return ParseTemplate(template, report);
        }

        static string ParseTemplate(string templateFile, Report report)
        {
            var templatePath = ResolvePath(templateFile);
            var template = File.ReadAllText(templatePath);
            return Razor.Parse(template, report);
        }

        static string ResolvePath(string template)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppSettings.EmailTemplageFolder, template);
        }
    }
}