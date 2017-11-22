namespace Fanex.NAPReport.SMALoss.Business.Services
{
    using System.Collections.Generic;
    using Fanex.NAPReport.SMALoss.Business.Criteria;
    using Fanex.NAPReport.SMALoss.Business.Entities;

    public class SmaLossService : ServiceBase, ISMALossService
    {
        /// <summary>
        /// Gets the SMA list that reached the max loss limit.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>List of SMALoss objects.</returns>
        public IEnumerable<SMALoss> GetNotifiedSMALoss(MonitorListCriteria criteria)
        {
            return GetEntites<SMALoss>(criteria);
        }

        /// <summary>
        /// Gets the list of set loss levels.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>List of SMALossLevel objects.</returns>
        public IEnumerable<SMALossLevel> GetSMALossLevels(LimitLossActionConfigCriteria criteria)
        {
            return GetEntites<SMALossLevel>(criteria);
        }
    }
}