// -----------------------------------------------------------------------
// <copyright file="SaveFile.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Utilities
{
    using System.IO;
    using System.Net;
    using Fanex.Common;
    using NPOI.SS.UserModel;
    using OfficeOpenXmlCrypto;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SaveFile
    {
        public static void SaveExcelFile(string exportDataDirectory, string fileName, IWorkbook workbook, string ExcelPassword = "")
        {
            if (AppSettingBase.IsAuthenticate)
            {
                SaveFileWithAuthenticate(exportDataDirectory, fileName, workbook, ExcelPassword);
            }
            else
            {
                SaveFileWithNoneAuthenticate(exportDataDirectory, fileName, workbook, ExcelPassword);
            }
        }

        private static void SaveFileWithNoneAuthenticate(string exportDataDirectory, string fileName, IWorkbook workbook, string ExcelPassword)
        {
            if (!Directory.Exists(exportDataDirectory))
            {
                Directory.CreateDirectory(exportDataDirectory);
            }

            exportDataDirectory += @"\" + fileName;

            FileInfo file = new FileInfo(exportDataDirectory);

            workbook.Write(new FileStream(exportDataDirectory, FileMode.Create)); 

            OfficeCryptoStream stream1 = OfficeCryptoStream.Open(exportDataDirectory);
            stream1.Password = ExcelPassword;
            stream1.SaveAs(exportDataDirectory);
            stream1.Close();
            stream1.Dispose();            
        }

        private static void SaveFileWithAuthenticate(string exportDataDirectory, string fileName, IWorkbook workbook, string ExcelPassword)
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
                if (!Directory.Exists(exportDataDirectory))
                {
                    Directory.CreateDirectory(exportDataDirectory);
                }

                exportDataDirectory += @"\" + fileName;

                FileInfo file = new FileInfo(exportDataDirectory); 

                workbook.Write(new FileStream(exportDataDirectory, FileMode.Create)); 

                OfficeCryptoStream stream1 = OfficeCryptoStream.Open(exportDataDirectory);
                stream1.Password = ExcelPassword;
                stream1.SaveAs(exportDataDirectory);
                stream1.Close();
                stream1.Dispose();                
            }
        }
    }
}
