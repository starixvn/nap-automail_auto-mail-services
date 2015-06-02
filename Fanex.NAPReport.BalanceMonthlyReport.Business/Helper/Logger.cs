using System;
using System.IO;

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Helper
{
    public class Logger
    {
        public static void WriteLog(string message)
        {
            var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Log");
            if (!directoryInfo.Exists) directoryInfo.Create();
            var fileInfo = new FileInfo(directoryInfo.FullName + "\\" + "Auto Mail Log ("
                + DateTime.Now.Year.ToString() + "-"
                + DateTime.Now.Month.ToString() + "-"
                + DateTime.Now.Day.ToString()
                + ").log");
            if (!fileInfo.Exists)
                fileInfo.Create().Close();
            using (var streamWriter = new StreamWriter(fileInfo.FullName))
            {
                streamWriter.WriteLine(message);
            }
        }
    }
}
