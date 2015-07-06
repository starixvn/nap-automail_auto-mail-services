// -----------------------------------------------------------------------
// <copyright file="RMSection.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Configurations.RM
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RMSection : ConfigurationSection
    {
        public enum Reports
        {
            MatchWinLoss2a = 0
        }

        public enum RMType
        {
            EightRM = 0,
            BARM = 1
        }

        public static RMSection Instance
        {
            get
            {
                var storeProcedureSection =
                 ConfigurationManager.GetSection("RMSection") as RMSection;
                return storeProcedureSection;
            }
        }

        [ConfigurationProperty("Report", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RMCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public RMCollection Report
        {
            get
            {
                var storeProcedureCollection =
                    (RMCollection)base["Report"];
                return storeProcedureCollection;
            }
        }
    }
}