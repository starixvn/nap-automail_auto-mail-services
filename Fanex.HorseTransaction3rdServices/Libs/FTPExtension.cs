namespace Fanex.NAP.HorseTransaction3rd.Business.Libs
{
    using System;
    using System.IO;
    using System.Net;

    public class FTPExtension
    {
        public static string FtpAcc { get; set; }

        public static string FtpPass { get; set; }

        public static string FtpHost { get; set; }

        public static string FtpFolder { get; set; }

        public static void DownLoadFile(string localPath, string fileName)
        {
            FtpWebRequest requestFileDownload = null;
            FtpWebResponse responseFileDownload = null;

            try
            {
                var ftpFullPath = string.Format("ftp://{0}{1}{2}", FtpHost, FtpFolder, fileName);                
                LogService.Write("FTPExtension.DownLoadFile", string.Format("get file: ({0})", ftpFullPath));

                requestFileDownload = (FtpWebRequest)WebRequest.Create(ftpFullPath);
                requestFileDownload.Credentials = new NetworkCredential(FtpAcc, FtpPass);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();

                var responseStream = responseFileDownload.GetResponseStream();
                var writeStream = new FileStream(localPath + fileName, FileMode.Create);

                int length = 2048;
                byte[] buffer = new byte[length];
                var bytesRead = responseStream.Read(buffer, 0, length);
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, length);
                }

                writeStream.Close();
            }
            catch (Exception exceptionObj)
            {
                LogService.Write("FTPExtension.DownLoadFile", string.Format("Error: ({0}) {1}", fileName, exceptionObj.Message));                
            }
            finally
            {                
                requestFileDownload = null;
                responseFileDownload = null;
            }
        }
    }
}
