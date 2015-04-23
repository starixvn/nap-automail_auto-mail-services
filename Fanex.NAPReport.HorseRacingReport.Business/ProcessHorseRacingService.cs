// -----------------------------------------------------------------------
// <copyright file="ProcessHorseRacingService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Fanex.NAPReport.HorseRacingReport.Business.Criteria;
    using Fanex.NAPReport.HorseRacingReport.Business.Resources;
    using Fanex.NAPReport.HorseRacingReport.Business.Service;
    using Fanex.Service.Core.Extensions;
    using Ionic.Zip;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ProcessHorseRacingService
    {
        private IHorseRacingReportService _horseRacingReportService;

        public string ReportZipFolderPath
        {
            get;
            set;
        }

        /// <summary>
        /// Processes the horse racing report service.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        public void ProcessHorseRacingReportService(TimeRangeCriteria criteria)
        {
            _horseRacingReportService = new HorseRacingReportService(criteria);

            try
            {
                var mainRacingReportListCriteria = GetListMainReportCriteria(criteria);
                var detailReacingListCriteria = GetListDetailReportCriteria(criteria);

                foreach (var mainRacingReportCriteria in mainRacingReportListCriteria)
                {
                    var mainReport = _horseRacingReportService.GetMainReport(mainRacingReportCriteria);
                    _horseRacingReportService.ExportMainReport(mainReport, mainRacingReportCriteria.ReportType, mainRacingReportCriteria.WinLostDate, criteria);
                }

                foreach (var detailRacingReportCriteria in detailReacingListCriteria)
                {
                    var detailReport = _horseRacingReportService.GetDetailReport(detailRacingReportCriteria);
                    _horseRacingReportService.ExportDetailReport(detailReport, detailRacingReportCriteria.PartnerId, detailRacingReportCriteria.WinLostDate, criteria);
                }

                string reportZipFolderName = CreateZipReportFolder(criteria);
                ReportZipFolderPath = Path.Combine(AppSettings.ExportDataDirectory, reportZipFolderName);

                string[] listFiles = GetFileToCompressed(_horseRacingReportService.GetReportDirectory);
                using (ZipFile zip = new ZipFile())
                {
                    foreach (string file in listFiles)
                    {
                        zip.AddFile(file, string.Empty);
                    }

                    zip.Save(ReportZipFolderPath);
                }

                Directory.Delete(_horseRacingReportService.GetReportDirectory, true);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the time range criteria.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>Time range of report.</returns>
        public TimeRangeCriteria GetTimeRangeCriteria()
        {
            DateTime currentTime = DateTime.Today;
            TimeRangeCriteria criteria = new TimeRangeCriteria();
            if (currentTime.DayOfWeek == DayOfWeek.Thursday)
            {
                criteria.FromDate = currentTime.AddDays(-3);
            }

            if (currentTime.DayOfWeek == DayOfWeek.Monday)
            {
                criteria.FromDate = currentTime.AddDays(-4);
            }

            criteria.ToDate = currentTime.AddDays(-1);

            return criteria;
        }

        /// <summary>
        /// Gets the list main report criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List main report criteria.</returns>
        private List<GetMainRacingReportCriteria> GetListMainReportCriteria(TimeRangeCriteria criteria)
        {
            List<GetMainRacingReportCriteria> mainReportCriteria = new List<GetMainRacingReportCriteria>();
            int numberDay = criteria.ToDate.Subtract(criteria.FromDate).Days;
            for (int i = 0; i <= numberDay; i++)
            {
                GetMainRacingReportCriteria horseRacingForUKToteCriteria = new GetMainRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-i).Date,
                    ReportType = 1
                };

                GetMainRacingReportCriteria horseRacingForSAToteCriteria = new GetMainRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-i).Date,
                    ReportType = 6
                };

                mainReportCriteria.Add(horseRacingForUKToteCriteria);
                mainReportCriteria.Add(horseRacingForSAToteCriteria);
            }

            return mainReportCriteria;
        }

        /// <summary>
        /// Gets the list detail report criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List detail report criteria.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private List<GetDetailRacingReportCriteria> GetListDetailReportCriteria(TimeRangeCriteria criteria)
        {
            List<GetDetailRacingReportCriteria> detailReportCriteria = new List<GetDetailRacingReportCriteria>();
            int numberDay = criteria.ToDate.Subtract(criteria.FromDate).Days;
            for (int i = 0; i <= numberDay; i++)
            {
                GetDetailRacingReportCriteria firstHorseRacingForAUSToteCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-i).Date,
                    PartnerId = 5
                };

                GetDetailRacingReportCriteria secondHorseRacingForAUSToteCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-i).Date,
                    PartnerId = 6
                };

                GetDetailRacingReportCriteria firstHorseRacingForGreyHoundCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-i).Date,
                    PartnerId = 103
                };

                GetDetailRacingReportCriteria secondHorseRacingForGreyHoundCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-i).Date,
                    PartnerId = 104
                };

                GetDetailRacingReportCriteria firstHorseRacingForHarnessCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-i).Date,
                    PartnerId = 201
                };

                GetDetailRacingReportCriteria secondHorseRacingForHarnessCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-i).Date,
                    PartnerId = 202
                };

                detailReportCriteria.Add(firstHorseRacingForAUSToteCriteria);
                detailReportCriteria.Add(secondHorseRacingForAUSToteCriteria);
                detailReportCriteria.Add(firstHorseRacingForGreyHoundCriteria);
                detailReportCriteria.Add(secondHorseRacingForGreyHoundCriteria);
                detailReportCriteria.Add(firstHorseRacingForHarnessCriteria);
                detailReportCriteria.Add(secondHorseRacingForHarnessCriteria);
            }

            return detailReportCriteria;
        }

        private string[] GetFileToCompressed(string directoryPath)
        {
            List<string> fileNames = new List<string>();
            foreach (var path in Directory.GetFiles(directoryPath))
            {
                fileNames.Add(path);
            }

            return fileNames.ToArray();
        }

        private string CreateZipReportFolder(TimeRangeCriteria criteria)
        {
            string reportFrom = criteria.FromDate.ToString("d MMM");
            string reportTo = criteria.ToDate.ToString("d MMM");
            return string.Format(RacingResource.Racing_Report_Directory, reportFrom, reportTo);
        }
    }
}