// -----------------------------------------------------------------------
// <copyright file="IHorseRacingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business.Service
{
    using Fanex.NAPReport.HorseRacingReport.Business.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IHorseRacingService
    {
        string ReportZipFolderPath
        {
            get;
            set;
        }

        string FileName 
        { 
            get; 
            set; 
        }

        TimeRangeCriteria GetTimeRangeCriteria();

        void GetData(TimeRangeCriteria criteria);
    }
}
