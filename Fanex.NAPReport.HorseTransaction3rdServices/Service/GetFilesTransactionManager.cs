namespace Fanex.NAPReport.HorseTransaction3rd.Business.Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using DNAide.Utils;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Configurations;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Libs;

    public class GetFilesTransactionManager : IGetFilesTransactionManager
    {
        private string _mask = string.Empty;

        private HorseRacing3rdService _service;      

        public GetFilesTransactionManager()
        {
            _service = new HorseRacing3rdService();
            MailList = new Dictionary<string, string>();
        }

        public GetFilesTransactionManager(HorseRacing3rdService service)
        {
            _service = service;
            MailList = new Dictionary<string, string>();
        }

        public Dictionary<string, string> MailList { get; set; }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void GetFilesTransaction()
        {
            var saveFilePath = string.Empty;
            var fileName = string.Empty;

            foreach (RacingElement racing in RacingSection.Instance.RacingConfigs)
            {
                if (racing.Active)
                {
                    saveFilePath = racing.SaveFilePath;

                    fileName = racing.FileName;
                    var ftpHost = racing.FtpHost;
                    var ftpAcc = racing.FtpAcc;
                    var ftpPass = Encryption.CfsDeCrypt(racing.FtpPass);
                    var ftpDomain = racing.FtpDomain;
                    _mask = racing.FtpFolder;

                    if (AppSetting.TestFlag)
                    {
                        var names = string.Format("<span style='padding-left:20px'>{0}. {1}-{2}.txt</span><br/>", 1, fileName, AppSetting.TestDate.ToString("yyyy-MM-dd"));
                        SendEmailNotificationSuccessful(names, saveFilePath, racing);
                    }
                    else
                    {
                        var ftp = new FTPFactory();
                        if (AppSetting.FtpLocal)
                        {
                            FTPExtension.FtpAcc = (AppSetting.IsLocal ? ftpDomain + "\\" : string.Empty) + ftpAcc;
                            FTPExtension.FtpPass = ftpPass;
                            FTPExtension.FtpHost = ftpHost;
                            FTPExtension.FtpFolder = _mask;
                        }
                        else
                        {
                            ftp.SetDebug(true);
                            ftp.SetRemoteHost(ftpHost);
                            if (AppSetting.IsLocal)
                            {
                                ftp.SetRemoteUser(ftpDomain + @"\" + ftpAcc);
                            }
                            else
                            {
                                ftp.SetRemoteUser(ftpAcc);
                            }

                            ftp.SetRemotePass(ftpPass);
                            ftp.Login();
                            ftp.SetBinaryMode(true);
                        }

                        var countNewFile = 0;
                        var arrFillter = new ArrayList();
                        var arrFileInserted = new ArrayList();

                        var begin = 1;
                        var end = AppSetting.RangeDate;

                        for (var i = begin; i < end; i++)
                        {
                            var s = fileName + "-" +
                                       FormatNumber(DateTime.Today.AddDays(-i).Year) + "-" +
                                       FormatNumber(DateTime.Today.AddDays(-i).Month) + "-" +
                                       FormatNumber(DateTime.Today.AddDays(-i).Day) + ".txt";

                            arrFillter.Add(s);
                        }

                        foreach (string file in arrFillter)
                        {
                            var filename = file.Replace("\r", string.Empty);
                            if (filename.Length > 0)
                            {
                                var fullpath = saveFilePath + @"\" + filename;
                                var isExistedFile = IsExistedFile(fullpath, saveFilePath);

                                if (!isExistedFile)
                                {
                                    try
                                    {
                                        if (AppSetting.FtpLocal)
                                        {
                                            FTPExtension.DownLoadFile(saveFilePath, filename);
                                        }
                                        else
                                        {
                                            ftp.Download(string.Format("{0}/{1}", _mask, filename), saveFilePath);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogService.Write("GetFilesTransaction", string.Format("Download File Fail: ({0}) {1}", fileName, ex.Message));
                                        SendEmailNotification("Get Ftp File: " + ex.Message, saveFilePath, racing);
                                    }

                                    var f = InsertDataFromFileToDb(fullpath, saveFilePath, racing);
                                    if (!f)
                                    {
                                        LogService.Write("GetFilesTransaction", string.Format("Maybe there is no data in dump file ({0} --> auto delete this file", filename));
                                        DeleteFile(fullpath);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Insert data completed");
                                        countNewFile++;
                                        arrFileInserted.Add(filename);
                                    }
                                }
                            }
                        }

                        if (!AppSetting.FtpLocal)
                        {
                            ftp.Close();
                            ftp = null;
                        }

                        var names = string.Empty;
                        var index = 0;
                        foreach (string file in arrFileInserted)
                        {
                            if (file.Length > 0)
                            {
                                index++;
                                names += "<span style='padding-left:20px'>" + index + ". " + file + "</span><br/>";
                            }
                        }

                        if (countNewFile > 0)
                        {
                            SendEmailNotificationSuccessful(names, saveFilePath, racing);
                        }
                        else
                        {
                            var date = DateTime.Today.AddDays(-1);
                            var criteria = new GetRacingWinLossCriteria() { ReportType = Convert.ToInt32(racing.Id), WinLostDate = date };
                            var result = _service.WinlostHR_OpenTote(criteria);

                            SendEmailNotification(
                                "Can't download any files for " + racing.Name + " because maybe they don't update new file(" + GetMissFileName(fileName) + ") on this day",
                                                                     saveFilePath,
                                                                     racing);
                        }
                    }
                }
            }
        }

        private string FormatNumber(int i)
        {
            var str = string.Empty;
            if (i.ToString().Length == 1)
            {
                str = "0" + i;
            }
            else
            {
                str = i.ToString();
            }

            return str;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private bool InsertDataFromFileToDb(string fullpath, string filePath, RacingElement racing)
        {
            var f = false;
            try
            {
                var str = ReadFile(fullpath, filePath, racing).Replace("\n", "^");
                string[] rows = str.Split('^');
                var indexRow = 0;

                string[] param = { "merchant_id", "merchant_name", "cre_timestamp", "provider", "transaction_id", "pool_oid", "meeting_name", "bet_type", "leg_races", "formatted_selections", "bet_options", "tsn", "currency", "stake", "cost", "payout", "other_currency", "other_cost", "other_payout", "cancel_amount", "reconciled", "cancelled", "has_refund", "has_payout", "client_cashed", "merchant_reference", "status", "rate" };
                int[] datatypes = { 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1 };

                foreach (var row in rows)
                {
                    var hashtable = new Hashtable();
                    string[] cells = row.Split('|');

                    if (cells.Length >= 2)
                    {
                        for (int i = 0; i < param.Length; i++)
                        {
                            if (cells[i].Length > 0)
                            {
                                if (datatypes[i] == 0)
                                {
                                    hashtable.Add(param[i], cells[i].Replace("\r", string.Empty));
                                }
                                else
                                {
                                    hashtable.Add(param[i], DNAide.Core.CastAide.AsDecimal(cells[i].Replace("\r", string.Empty)));
                                }
                            }
                        }

                        if (indexRow > 0)
                        {
                            var criteria = new InsertHorseRacingCriteria() { ParameterList = hashtable };
                            f = _service.InsertDataFromFileToDb(criteria);
                        }

                        indexRow++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Write("InsertDataFromFileToDb", string.Format("Insert Data Fail: {0}", ex.Message));
                SendEmailNotification("Insert Data: " + ex.Message, filePath, racing);
            }

            return f;
        }

        protected virtual void SendEmailNotification(string content, string filePath, RacingElement racing)
        {
            try
            {
                string emailTemplatePath = AppDomain.CurrentDomain.BaseDirectory + AppSetting.TemplateOfUnsuccessEmailDirectory;
                string template = File.ReadAllText(emailTemplatePath);
                template = string.Format(template, content);

                MailList.Add(racing.Name, template);

                LogService.Write("SendEmailNotification", string.Format("Send mail Successfully: {0}<br/>{1}", Resource.Resource.SubjectOfMail, template));
            }
            catch (Exception ex)
            {
                var msg = "<span style='color:red'><b>Date: " + DateTime.Now.ToString() + "<br/>" + ex.Message + "</b></span><br/><br/><br/>";
                LogService.Write("SendEmailNotification", string.Format("Send mail notification: {0}", msg));
            }
        }

        protected virtual void SendEmailNotificationSuccessful(string filename, string filePath, RacingElement racing)
        {
            try
            {
                var compare = new ComparisionManager();
                var comparisionResult = string.Empty;
                if (AppSetting.TestFlag)
                {
                    comparisionResult += compare.GetComparision(AppSetting.TestDate, Convert.ToInt32(racing.Id)) + "</br></br>";
                }
                else
                {
                    comparisionResult += compare.GetComparision(DateTime.Today.AddDays(-1), Convert.ToInt32(racing.Id)) + "</br></br>";
                }

                string emailTemplatePath = AppDomain.CurrentDomain.BaseDirectory + AppSetting.TemplateOfSuccessEmailDirectory;
                string strContent = File.ReadAllText(emailTemplatePath);
                strContent = string.Format(strContent, racing.Name, filename, comparisionResult);

                MailList.Add(racing.Name, strContent);

                LogService.Write("SendEmailNotificationSuccessful", string.Format("Send mail successfully: {0}<br/>{1}", Resource.Resource.SubjectOfMail, strContent));
            }
            catch (Exception ex)
            {
                var msg = "<span style='color:red'><b>Date: " + DateTime.Now.ToString() + "<br/>" + ex.Message + "</b></span><br/><br/><br/>";
                LogService.Write("SendEmailNotificationSuccessful", string.Format("Send mail fail: {0}", msg));
            }
        }

        protected virtual bool IsExistedFile(string filename, string filePath)
        {
            var f = false;

            if (AppSetting.IsLocal)
            {
                filename = filename.Replace(_mask, string.Empty);
            }

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string[] files = Directory.GetFiles(filePath);
            foreach (var file in files)
            {
                f = File.Exists(filename);
                if (f)
                {
                    break;
                }
            }

            return f;
        }

        protected virtual void DeleteFile(string fullName)
        {
            if (File.Exists(fullName))
            {
                File.Delete(fullName);
            }
        }

        protected virtual string ReadFile(string fullpath, string filePath, RacingElement racing)
        {
            var html = new StringBuilder();
            try
            {
                if (File.Exists(fullpath))
                {
                    var file = new FileStream(fullpath, FileMode.OpenOrCreate, FileAccess.Read);
                    var sr = new StreamReader(file);
                    html.Append(sr.ReadToEnd());
                    sr.Close();
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                LogService.Write("ReadFile", string.Format("Fail: {0}", ex.Message));
                SendEmailNotification("ReadFile: " + ex.Message, filePath, racing);
            }

            return html.ToString();
        }

        protected virtual string GetMissFileName(string fileName)
        {
            var str = string.Empty;
            var yy = DateTime.Today.Year;
            var mm = DateTime.Today.Month;
            var dd = DateTime.Today.AddDays(-1).Day;
            str = fileName + "-" + yy + "-" + ((mm < 10) ? "0" + mm : mm.ToString()) + "-" + dd + ".txt";
            return str;
        }
    }
}
