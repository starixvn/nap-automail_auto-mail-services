namespace Fanex.NAPReport.SMALoss.Business.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAPReport.SMALoss.Business.Criteria;
    using Fanex.NAPReport.SMALoss.Business.Entities;

    public interface ISMALossService
    {
        Recipients GetReceivers(MailingListSelCriteria criteria);

        /// <summary>
        /// Gets the SMA list that reached the max loss limit.
        /// </summary>
        /// <returns>List of SMALoss objects.</returns>
        IEnumerable<SMALoss> GetNotifiedSMALoss();

        /// <summary>
        /// Gets the list of set loss levels.
        /// </summary>
        /// <returns>List of SMALossLevel objects.</returns>
        IEnumerable<SMALossLevel> GetSMALossLevels();

        /// <summary>
        /// Updates the notified status.
        /// </summary>
        /// <param name="updatingData">The updating data object contains up-to-date data.</param>
        void UpdateNotifiedStatus(UpdatingNotifiedSMALoss updatingData);
    }
}