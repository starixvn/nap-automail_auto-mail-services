namespace Fanex.NAPReport.HorseTransaction3rd.Business.Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Configurations;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Libs;
    using RazorEngine;
    using RazorEngine.Templating;
    using System.Linq;

    public class GetFilesTransactionManager : IGetFilesTransactionManager
    {
        private string _mask = string.Empty;

        private HorseRacing3rdService _service;

        public GetFilesTransactionManager()
        {
            _service = new HorseRacing3rdService();
            MailList = new List<string>();
        }

        public GetFilesTransactionManager(HorseRacing3rdService service)
        {
            _service = service;
            MailList = new List<string>();
        }

        public List<string> MailList { get; set; }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public bool GetFilesTransaction()
        {
            var saveFilePath = string.Empty;
            var fileName = string.Empty;
            var result = false;
            var logService = new LogServiceImp();

            foreach (RacingElement racing in RacingSection.Instance.RacingConfigs)
            {
                if (racing.Active)
                {
                    saveFilePath = racing.SaveFilePath;

                    fileName = racing.FileName;
                    var ftpHost = racing.FtpHost;
                    var ftpAcc = racing.FtpAcc;
                    var ftpPass = racing.FtpPass;
                    var ftpDomain = racing.FtpDomain;
                    _mask = racing.FtpFolder;

                    var ftp = new FTPFactory();
                    FTPExtension.FtpAcc = ftpAcc;
                    FTPExtension.FtpPass = ftpPass;
                    FTPExtension.FtpHost = ftpHost;
                    FTPExtension.FtpFolder = _mask;

                    var countNewFile = 0;
                    var arrFillter = new ArrayList();
                    var arrFileInserted = new ArrayList();
                    
                    var tnxFromDate = DateTime.Today.AddDays(-1);
                    var tnxToDate = DateTime.Today.AddDays(-1);
                    if (AppSetting.IsManual)
                    {
                        tnxFromDate = AppSetting.FromDate;
                        tnxToDate = AppSetting.ToDate;
                    }

                    for (var tnxDate = tnxFromDate; tnxFromDate <= tnxToDate; tnxFromDate = tnxFromDate.AddDays(1))
                    {
                        var s = $"{fileName}-{tnxFromDate:yyyy-MM-dd}.txt";
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
                                FTPExtension.DownLoadFile(saveFilePath, filename);

                                var f = InsertDataFromFileToDb(fullpath, saveFilePath, racing);
                                if (!f)
                                {
                                    DeleteFile(fullpath);
                                }
                                else
                                {
                                    countNewFile++;
                                    arrFileInserted.Add(filename);
                                }
                            }
                        }
                    }

                    ftp.Close();
                    ftp = null;

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
                        result = true;
                        SendEmailNotificationSuccessful(names, saveFilePath, racing);
                    }
                    else
                    {
                        result = false;
                        SendEmailNotification("Can't download any files for " + racing.Name + " because maybe they don't update new file(" + GetMissFileName(fileName) + ") on this day, or they have already been downloaded.", saveFilePath, racing);
                    }
                }
            }

            return result;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private bool InsertDataFromFileToDb(string fullpath, string filePath, RacingElement racing)
        {
            var f = false;
            var str = ReadFile(fullpath, filePath, racing).Replace("\n", "^");
            string[] rows = str.Split('^');
            var indexRow = 0;

            string[] param = { "merchant_id", "merchant_name", "cre_timestamp", "provider", "transaction_id", "pool_oid", "meeting_name", "bet_type", "leg_races", "formatted_selections", "bet_options", "tsn", "currency", "stake", "cost", "payout", "other_currency", "other_cost", "otherpayout", "cancelamount", "reconciled", "cancelled", "has_refund", "has_payout", "client_cashed", "merchant_reference", "status", "rate" };
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

            return f;
        }

        protected virtual void SendEmailNotification(string content, string filePath, RacingElement racing)
        {
            string emailTemplatePath = AppDomain.CurrentDomain.BaseDirectory + AppSetting.TemplateOfUnsuccessEmailDirectory;
            string template = File.ReadAllText(emailTemplatePath);
            template = Engine.Razor.RunCompile(template, template);
            template = string.Format(template, content);

            MailList.Add(template);
        }

        protected virtual void SendEmailNotificationSuccessful(string filename, string filePath, RacingElement racing)
        {
            var compare = new ComparisionManager();
            var comparisionResult = string.Empty;
            if (AppSetting.IsManual)
            {
                comparisionResult += compare.GetComparision(AppSetting.FromDate, Convert.ToInt32(racing.Id)) + "</br></br>";
            }
            else
            {
                comparisionResult += compare.GetComparision(DateTime.Today.AddDays(-1), Convert.ToInt32(racing.Id)) + "</br></br>";
            }

            string emailTemplatePath = AppDomain.CurrentDomain.BaseDirectory + AppSetting.TemplateOfSuccessEmailDirectory;
            string strContent = File.ReadAllText(emailTemplatePath);
            strContent = Engine.Razor.RunCompile(strContent, strContent);
            strContent = string.Format(strContent, racing.Name, filename, comparisionResult);

            MailList.Add(strContent);
        }

        protected virtual bool IsExistedFile(string filename, string filePath)
        {
            var f = false;

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
            if (File.Exists(fullpath))
            {
                var file = new FileStream(fullpath, FileMode.OpenOrCreate, FileAccess.Read);
                var sr = new StreamReader(file);
                html.Append(sr.ReadToEnd());
                sr.Close();
                file.Close();
            }

            return html.ToString();
        }

        protected virtual string GetMissFileName(string fileName)
        {
            return $"{fileName}-{DateTime.Today.AddDays(-1):yyyy-MM-dd}.txt";
        }
    }
}