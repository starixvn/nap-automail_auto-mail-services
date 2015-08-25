// -----------------------------------------------------------------------
// <copyright file="CustomBettypeSection.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Configurations.CustomBettype
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomBettypeSection : ConfigurationSection
    {
        public static CustomBettypeSection Instance
        {
            get
            {
                var storeProcedureSection =
                 ConfigurationManager.GetSection("CustomBettypeSection") as CustomBettypeSection;
                return storeProcedureSection;
            }
        }

        [ConfigurationProperty("BettypeName", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CustomBettypeCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CustomBettypeCollection BettypeName
        {
            get
            {
                var storeProcedureCollection =
                    (CustomBettypeCollection)base["BettypeName"];
                return storeProcedureCollection;
            }
        }

        [ConfigurationProperty("BettypeNameByBettypeRange", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CustomBettypeCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CustomBettypeCollection BettypeNameByBettypeRange
        {
            get
            {
                var storeProcedureCollection =
                    (CustomBettypeCollection)base["BettypeNameByBettypeRange"];
                return storeProcedureCollection;
            }
        }

        [ConfigurationProperty("BettypeNameSuffix", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CustomBettypeCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CustomBettypeCollection BettypeNameSuffix
        {
            get
            {
                var storeProcedureCollection =
                    (CustomBettypeCollection)base["BettypeNameSuffix"];
                return storeProcedureCollection;
            }
        }

        [ConfigurationProperty("BettypeNameSuffixByBettypeRange", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CustomBettypeCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CustomBettypeCollection BettypeNameSuffixByBettypeRange
        {
            get
            {
                var storeProcedureCollection =
                    (CustomBettypeCollection)base["BettypeNameSuffixByBettypeRange"];
                return storeProcedureCollection;
            }
        }
    }
}