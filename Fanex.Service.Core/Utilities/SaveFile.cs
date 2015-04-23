// -----------------------------------------------------------------------
// <copyright file="SaveFile.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
using NPOI.SS.UserModel;
    using OfficeOpenXmlCrypto;
    using Fanex.Common;
    using System.Net;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SaveFile
    {
        public static string SaveFileWithNoneAuthenticate(string exportDataDirectoryKey, string fileName, IWorkbook workbook)
        {
            string getFileName = ExportDataDirectory(exportDataDirectoryKey);
            if (!Directory.Exists(getFileName))
            {
                Directory.CreateDirectory(getFileName);
            }

            getFileName += @"\" + fileName;

            FileInfo file = new FileInfo(getFileName);
            file.Delete();

            workbook.Write(new FileStream(getFileName, FileMode.Create));

            OfficeCryptoStream stream1 = OfficeCryptoStream.Open(getFileName);
            stream1.Password = AppSettingBase.ExcelPassword;
            stream1.SaveAs(getFileName);
            stream1.Close();

            return getFileName;
        }

        public static string SaveFileWithAuthenticate(string exportDataDirectoryKey, string fileName, IWorkbook workbook)
        {
            string ip = AppSettingBase.IP;
            string password = AppSettingBase.Password;
            string username = AppSettingBase.UserName;

            if (AppSettingBase.IsEncryptPassword)
            {
                password = Cryptography.CfsDeCrypt(password);
            }

            NetworkCredential networkCredential = new NetworkCredential(username, password);

            using (new NetworkConnection(@"\\" + ip, networkCredential))
            {
                string getFileName = ExportDataDirectory(exportDataDirectoryKey);
                if (!Directory.Exists(getFileName))
                {
                    Directory.CreateDirectory(getFileName);
                }

                getFileName += @"\" + fileName;

                FileInfo file = new FileInfo(getFileName);
                file.Delete();

                workbook.Write(new FileStream(getFileName, FileMode.Create));

                OfficeCryptoStream stream1 = OfficeCryptoStream.Open(getFileName);
                stream1.Password = AppSettingBase.ExcelPassword;
                stream1.SaveAs(getFileName);
                stream1.Close();

                return getFileName;
            }
        }

        private static string ExportDataDirectory(string exportDataDirectoryKey)
        {
            string path = AppSettingBase.GetSettingValueByKey(exportDataDirectoryKey);
            string month = DateTime.Today.Month < 10
                ? "0" + DateTime.Today.Month
                : DateTime.Today.Month + string.Empty;

            path += @"\" + DateTime.Today.Year + @"\" + month;

            return path;
        }
    }
}
