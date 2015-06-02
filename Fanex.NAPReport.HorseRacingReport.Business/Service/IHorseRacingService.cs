// -----------------------------------------------------------------------
// <copyright file="IHorseRacingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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

        TimeRangeCriteria GetTimeRangeCriteria();

        void GetData(TimeRangeCriteria criteria);
    }
}
