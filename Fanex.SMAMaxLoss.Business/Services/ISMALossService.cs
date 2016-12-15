namespace Fanex.NAPReport.SMALoss.Business.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAPReport.SMALoss.Business.Criteria;
    using Fanex.NAPReport.SMALoss.Business.Entities;

    public interface ISMALossService
    {
        IEnumerable<SMALoss> GetNotifiedSMALoss(MonitorListCriteria criteria);

        IEnumerable<SMALossLevel> GetSMALossLevels(LimitLossActionConfigCriteria criteria);
    }
}