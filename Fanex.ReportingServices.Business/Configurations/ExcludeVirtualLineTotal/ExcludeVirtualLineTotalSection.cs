// -----------------------------------------------------------------------
// <copyright file="ExcludeVirtualLineTotalSection.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Configurations.ExcludeVirtualLineTotal
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExcludeVirtualLineTotalSection : ConfigurationSection
    {
        public static ExcludeVirtualLineTotalSection Instance
        {
            get
            {
                var storeProcedureSection =
                 ConfigurationManager.GetSection("ExcludeVirtualLineTotalSection") as ExcludeVirtualLineTotalSection;
                return storeProcedureSection;
            }
        }

        [ConfigurationProperty("Bettypes", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ExcludeVirtualLineTotalCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ExcludeVirtualLineTotalCollection Bettypes
        {
            get
            {
                var storeProcedureCollection =
                    (ExcludeVirtualLineTotalCollection)base["Bettypes"];
                return storeProcedureCollection;
            }
        }

        [ConfigurationProperty("BettypeRangeBySport", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ExcludeVirtualLineTotalCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ExcludeVirtualLineTotalCollection BettypeRangeBySport
        {
            get
            {
                var storeProcedureCollection =
                    (ExcludeVirtualLineTotalCollection)base["BettypeRangeBySport"];
                return storeProcedureCollection;
            }
        }

        [ConfigurationProperty("ProductNames", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ExcludeVirtualLineTotalCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ExcludeVirtualLineTotalCollection ProductNames
        {
            get
            {
                var storeProcedureCollection =
                    (ExcludeVirtualLineTotalCollection)base["ProductNames"];
                return storeProcedureCollection;
            }
        }
    }
}