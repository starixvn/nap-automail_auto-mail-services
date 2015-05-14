namespace Fanex.NAPReport.BalanceReport.Core
{
    using System;
    using System.Globalization;
    using System.IO;
    using Fanex.NAPReport.BalanceReport.Model;

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
            return ParseTemplate("BalanceReport.htm", report);
        }

        public static string LoadBody(Report report)
        {
            return ParseTemplate("BalanceReport.htm", report);
        }

        static string ParseTemplate(string templateFile, Report report)
        {
            var templatePath = ResolvePath(templateFile);
            var template = File.ReadAllText(templatePath);
            return Razor.Parse(template, report);
        }

        static string ResolvePath(string template)
        {
            return AppDomain.CurrentDomain.BaseDirectory + "Email/BalanceReport/" + template;
        }
    }
}