namespace Fanex.NAPReport.SMALoss.Business.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.Framework.Data;
    using Fanex.NAPReport.SMALoss.Business.Entities;

    public class UpdatingNotifiedSMALossCriteria : CriteriaBase
    {
        public UpdatingNotifiedSMALossCriteria(IEnumerable<SMALoss> turnovers)
        {
            this.SMAIDs = string.Empty;

            if (turnovers != null && turnovers.Any())
            {
                foreach (SMALoss item in turnovers)
                {
                    this.SMAIDs += item.CustID + ",";
                }

                this.SMAIDs = this.SMAIDs.Remove(this.SMAIDs.Length - 1);
            }
        }

        public string SMAIDs { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(this.SMAIDs);
        }
    }
}