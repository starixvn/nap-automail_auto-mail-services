// <copyright file="ExportExcelService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement
{
    using System;
    using System.IO;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Common;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Configuration;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Extensions;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Resources;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Export excel for virtual sport's win loss.
    /// </summary>
    public class ExportExcelService : IExportExcelService
    {
        private HSSFWorkbook _workbook;

        /// <summary>
        /// Processes the data to send mail.
        /// </summary>
        /// <returns>Email send successful or not.</returns>
        public bool ProcessDataToSendMail()
        {
            bool isSendEmailSuccess = false;

            try
            {
                ////1. Export excel
                string fileAttachment = this.ExportExcelToDisk();

                ////2. Send email and attach excel file
                Mailing.MailingWinLossByVirtualSport(fileAttachment);

                isSendEmailSuccess = true;
            }
            catch (Exception ex)
            {
                isSendEmailSuccess = false;
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return isSendEmailSuccess;
        }

        /// <summary>
        /// Exports the excel to disk.
        /// </summary>
        /// <returns>Excel file path.</returns>
        public string ExportExcelToDisk()
        {
            this._workbook = new HSSFWorkbook();
            FileStream fileStream = null;

            var exportSummaryService = new ExportExcelWinLossSummaryService();
            exportSummaryService.ExportExcelWinlossSummary(ref _workbook);

            var exportDetailService = new ExportExcelWinLossDetailService();
            exportDetailService.ExportExcelWinlossDetail(ref _workbook);

            var yesterday = DateTime.Now.AddDays(-1);
            var reportDirectory = this.CreateWinLossByVirtualSportReportDirectory(yesterday);
            string reportName = string.Format(
                WinLossVirtualSportResoure.VirtualSportWinLoss_ReportName,
                DateTime.Now.ToShortFormatedDateWithSlash().Replace('/', '_'));
            var reportPath = Path.Combine(reportDirectory, reportName);
            var fullReportPath = string.Format("{0}.{1}", reportPath, "xls");

            fileStream = new FileStream(
                    fullReportPath,
                    FileMode.Create,
                    FileAccess.Write);

            this._workbook.Write(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            return fullReportPath;
        }

        /// <summary>
        /// Creates the win loss by virtual sport report directory.
        /// </summary>
        /// <param name="virtualSportsDate">The virtual sports date.</param>
        /// <returns>Report file directory.</returns>
        internal string CreateWinLossByVirtualSportReportDirectory(DateTime virtualSportsDate)
        {
            string reportDataDirectory = Settings.ExportDataDirectory;

            DirectoryExtensions.CheckAndCreateDirectory(reportDataDirectory);

            string directoryByYearAndMonth = string.Format(
                "{0}",
                virtualSportsDate.ToString("yyyy"));
            string directoryByYearAndMonthPath = Path.Combine(reportDataDirectory, directoryByYearAndMonth);
            DirectoryExtensions.CheckAndCreateDirectory(directoryByYearAndMonthPath);

            string directoryByReport = string.Format(
                "{0}",
                virtualSportsDate.ToString("MM"));
            string directoryByReportPath = Path.Combine(directoryByYearAndMonthPath, directoryByReport);
            DirectoryExtensions.CheckAndCreateDirectory(directoryByReportPath);

            return directoryByReportPath;
        }
    }
}