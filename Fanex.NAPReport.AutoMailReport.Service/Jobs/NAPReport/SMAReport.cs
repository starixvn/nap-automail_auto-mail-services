// -----------------------------------------------------------------------
// <copyright file="WinLossByProducts.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.NAPReport
{
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.SMAReport.Business.Service;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAReport : JobBase
    {
        private ISMAReportService _smaReportService;

        public SMAReport()
        {
            _smaReportService = new SMAReportService();
        }

        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {           
            var mail = _smaReportService.SMAReport();
            
            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
                Result = new ResultMessage
                {
                    ErrCode = 0,
                    ErrMsg = string.Empty
                }
            };
        }

        public JobResultMessage Run()
        {
            var mail = _smaReportService.SMAReport();

            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                //Schedule = schedule,
                Result = new ResultMessage
                {
                    ErrCode = 0,
                    ErrMsg = string.Empty
                }
            };
        }
    }
}