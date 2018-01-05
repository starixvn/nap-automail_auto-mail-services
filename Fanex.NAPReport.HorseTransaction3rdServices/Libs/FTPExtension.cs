namespace Fanex.NAPReport.HorseTransaction3rd.Business.Libs
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

            var ftpFullPath = string.Format("ftp://{0}{1}{2}", FtpHost, FtpFolder, fileName);
            
            try
            {
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
                requestFileDownload = null;
                responseFileDownload = null;

                Fanex.Logging.LogService.Info($"Getting the files via Ftp Server {Environment.NewLine} - localPath:{localPath} {Environment.NewLine} - fileName: {fileName} {Environment.NewLine} - bytesRead: {bytesRead}");
            }
            catch (Exception ex)
            {                
                throw new Exception($"The file name {fileName} - {ex.Message}");
            }           
        }
    }
}