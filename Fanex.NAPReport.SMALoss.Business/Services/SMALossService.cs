namespace Fanex.NAPReport.SMALoss.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fanex.NAPReport.SMALoss.Business.Criteria;
    using Fanex.NAPReport.SMALoss.Business.Entities;
    using Fanex.Service.Core.Service;

    public class SMALossService : ISMALossService
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
        /// <returns>List of SMALoss objects.</returns>
        public IEnumerable<SMALoss> GetNotifiedSMALoss()
        {
            IEnumerable<SMALoss> result = null;

            using (var objDB = new ObjectDb("NAP_LimitLoss_MonitorList"))
            {
                result = objDB.Query<SMALoss>();
            }

            return result;
        }

        /// <summary>
        /// Gets the list of set loss levels.
        /// </summary>
        /// <returns>List of SMALossLevel objects.</returns>
        public IEnumerable<SMALossLevel> GetSMALossLevels()
        {
            IEnumerable<SMALossLevel> result = null;

            using (var objDB = new ObjectDb("NAP_LimitLoss_ActionConfigSel"))
            {
                result = objDB.Query<SMALossLevel>();
            }

            return result;
        }

        /// <summary>
        /// Updates the notified status.
        /// </summary>
        /// <param name="updatingData">The updating data object contains up-to-date data.</param>
        public void UpdateNotifiedStatus(UpdatingNotifiedSMALoss updatingData)
        {
            Hashtable output = null;

            using (var objDB = new ObjectDb("NAP_LimitLoss_IsNotifiedUpd"))
            {
                objDB.AddInParameter("SMAIDs", DbType.String, updatingData.SMAIDs);
                objDB.AddOutParameter("ErrCode", DbType.Int32, 4);
                objDB.AddOutParameter("ErrMsg", DbType.String, 400);

                objDB.ExecuteNonQuery();

                output = objDB.GetOuputParams();

                if (output.ContainsKey("errcode"))
                {
                    updatingData.ErrCode = Converter.ToInt(output["errorcode"]);
                }

                if (output.ContainsKey("errmsg"))
                {
                    updatingData.ErrMsg = Converter.ToString2(output["errmsg"]);
                }
            }
        }
    }
}