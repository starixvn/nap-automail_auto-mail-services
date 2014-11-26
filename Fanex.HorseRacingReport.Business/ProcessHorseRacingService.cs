// -----------------------------------------------------------------------
// <copyright file="ProcessHorseRacingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.HorseRacingReport.Business
{
    using System;
    using System.Collections.Generic;
    using Fanex.HorseRacingReport.Business.Criteria;
    using Fanex.HorseRacingReport.Business.Service;
    using Fanex.HorseRacingReport.Business.Repositories;
    using Fanex.HorseRacingReport.Business.Model;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Enums;
    using System.IO;
    using Ionic.Zip;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ProcessHorseRacingService
    {
        private IHorseRacingReportService _horseRacingReportService;
        private ITaskRepository _taskRepository;

        public void ProcessHorseRacingReportService(TimeRangeCriteria criteria)
        {
            _horseRacingReportService = new HorseRacingReportService(criteria);
            _taskRepository = new TaskRepository();
            TaskModel model = _taskRepository.GetCurrentTask();

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
                string reportZipFolderPath = Path.Combine(AppSettings.ExportDataDirectory, reportZipFolderName);
                string[] listFiles = GetFileToCompressed(_horseRacingReportService.GetReportDirectory);
                using (ZipFile zip = new ZipFile())
                {
                    foreach (string file in listFiles)
                    {
                        zip.AddFile(file, string.Empty);
                    }

                    zip.Save(reportZipFolderPath);
                }

                bool sendMailSuccess = Mailing.SendRacingReportEmail(reportZipFolderPath, criteria);
                if (sendMailSuccess)
                {
                    _taskRepository.UpdateTaskStatus(model.Id, Status.Successful);
                }
                else
                {
                    _taskRepository.UpdateTaskStatus(model.Id, Status.Failed);
                }
            }
            catch (Exception ex)
            {
                _taskRepository.UpdateTaskStatus(model.Id, Status.Failed);
                Mailing.ErrorEmailNotification(criteria);
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public TimeRangeCriteria GetTimeRangeCriteria(DateTime currentTime)
        {
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

        private List<GetMainRacingReportCriteria> GetListMainReportCriteria(TimeRangeCriteria criteria)
        {
            List<GetMainRacingReportCriteria> mainReportCriteria = new List<GetMainRacingReportCriteria>();
            int numberDay = criteria.ToDate.Subtract(criteria.FromDate).Days;
            for (int i = 0; i < numberDay; i++)
            {
                int numberPreviousDay = i + 1;
                GetMainRacingReportCriteria horseRacingForUKToteCriteria = new GetMainRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-numberPreviousDay).Date,
                    ReportType = 1
                };

                GetMainRacingReportCriteria horseRacingForSAToteCriteria = new GetMainRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-numberPreviousDay).Date,
                    ReportType = 6
                };

                mainReportCriteria.Add(horseRacingForUKToteCriteria);
                mainReportCriteria.Add(horseRacingForSAToteCriteria);
            }

            return mainReportCriteria;
        }

        private List<GetDetailRacingReportCriteria> GetListDetailReportCriteria(TimeRangeCriteria criteria)
        {
            List<GetDetailRacingReportCriteria> detailReportCriteria = new List<GetDetailRacingReportCriteria>();
            int numberDay = criteria.ToDate.Subtract(criteria.FromDate).Days;
            for (int i = 0; i < numberDay; i++)
            {
                int numberPreviousDay = i + 1;
                GetDetailRacingReportCriteria firstHorseRacingForAUSToteCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-numberPreviousDay).Date,
                    PartnerId = 5
                };

                GetDetailRacingReportCriteria secondHorseRacingForAUSToteCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-numberPreviousDay).Date,
                    PartnerId = 6
                };

                GetDetailRacingReportCriteria firstHorseRacingForGreyHoundCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-numberPreviousDay).Date,
                    PartnerId = 103
                };

                GetDetailRacingReportCriteria secondHorseRacingForGreyHoundCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-numberPreviousDay).Date,
                    PartnerId = 104
                };

                GetDetailRacingReportCriteria firstHorseRacingForHarnessCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-numberPreviousDay).Date,
                    PartnerId = 201
                };

                GetDetailRacingReportCriteria secondHorseRacingForHarnessCriteria = new GetDetailRacingReportCriteria
                {
                    WinLostDate = criteria.ToDate.AddDays(-numberPreviousDay).Date,
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
            string reportFrom = criteria.FromDate.ToShortFormatedDateWithSlash().Replace("/", "-");
            string reportTo = criteria.ToDate.ToShortFormatedDateWithSlash().Replace("/", "-");
            return string.Format("RacingReport_{0}_{1}.zip", reportFrom, reportTo);
        }
    }
}