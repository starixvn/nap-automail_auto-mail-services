// -----------------------------------------------------------------------
// <copyright file="FileHelper.cs" company="Nexcel">
// TODO: Steven.Nguyen
// </copyright>

namespace Robot.Report.RngCasino.Helper.Implement
{
    using System;
    using System.IO;

    /// <summary>
    /// File Extension.
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="content">The content to write log.</param>
        public static void WriteFile(string content)
        {
            string logPath = AppDomain.CurrentDomain.BaseDirectory;
            var outPath = string.Format(@"{0}\Logs\Error", logPath);

            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            outPath += string.Format(@"\{0:yyyy-MM-dd}.txt", DateTime.Now.Date);

            var fs = new FileStream(outPath, FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);

            sw.WriteLine(DateTime.Now + ": " + content);

            sw.Close();
            fs.Close();
        }
    }
}
