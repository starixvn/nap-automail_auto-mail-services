namespace Fanex.NAPReport.SMALoss.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fanex.NAPReport.SMALoss.Business.Criteria;
    using Fanex.NAPReport.SMALoss.Business.Entities;
    using Fanex.Service.Core.Service;

    public class SMALossService : ServiceBase, ISMALossService
    {
        /// <summary>
        /// Gets the receivers.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Recipients objects.</returns>
        public Recipients GetReceivers(MailingListSelCriteria criteria)
        {
            return GetEntity<Recipients>(criteria);
        }

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