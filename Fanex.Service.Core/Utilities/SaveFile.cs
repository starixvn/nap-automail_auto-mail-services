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
        public static void SaveExcelFile(string rootDataDirectory, string exportDataDirectory, string fileName, IWorkbook workbook, string excelPassword = "")
        {
            if (AppSettingBase.IsAuthenticate)
            {
                SaveFileWithAuthenticate(rootDataDirectory, exportDataDirectory, fileName, workbook, excelPassword);
            }
            else
            {
                SaveFileWithNoneAuthenticate(exportDataDirectory, fileName, workbook, excelPassword);
            }
        }

        private static void SaveFileWithNoneAuthenticate(string exportDataDirectory, string fileName, IWorkbook workbook, string excelPassword)
        {
            if (!Directory.Exists(exportDataDirectory))
            {
                Directory.CreateDirectory(exportDataDirectory);
            }

            exportDataDirectory += @"\" + fileName;

            FileInfo file = new FileInfo(exportDataDirectory);

            workbook.Write(new FileStream(exportDataDirectory, FileMode.Create)); 

            OfficeCryptoStream stream1 = OfficeCryptoStream.Open(exportDataDirectory);
            stream1.Password = excelPassword;
            stream1.SaveAs(exportDataDirectory);
            stream1.Close();
            stream1.Dispose();            
        }

        private static void SaveFileWithAuthenticate(string rootDataDirectory, string exportDataDirectory, string fileName, IWorkbook workbook, string excelPassword)
        {
            string password = AppSettingBase.Password;
            string username = AppSettingBase.UserName;

            if (AppSettingBase.IsEncryptPassword)
            {
                password = Cryptography.CfsDeCrypt(password);
            }

            NetworkCredential networkCredential = new NetworkCredential(username, password);

            using (new NetworkConnection(rootDataDirectory, networkCredential))
            {
                if (!Directory.Exists(exportDataDirectory))
                {
                    Directory.CreateDirectory(exportDataDirectory);
                }

                exportDataDirectory += @"\" + fileName;

                FileInfo file = new FileInfo(exportDataDirectory); 

                workbook.Write(new FileStream(exportDataDirectory, FileMode.Create)); 

                OfficeCryptoStream stream1 = OfficeCryptoStream.Open(exportDataDirectory);
                stream1.Password = excelPassword;
                stream1.SaveAs(exportDataDirectory);
                stream1.Close();
                stream1.Dispose();                
            }
        }
    }
}
