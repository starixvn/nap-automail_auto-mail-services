// -----------------------------------------------------------------------
// <copyright file="ExcludeVirtualLineTotalElement.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Configurations.ExcludeVirtualLineTotal
{
    using System.Configuration;
    using Fanex.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExcludeVirtualLineTotalElement : ConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMElement"/> class.
        /// </summary>
        public ExcludeVirtualLineTotalElement()
        {
        }

        [ConfigurationProperty("Id", IsRequired = false)]
        public int Id
        {
            get
            {
                return Converter.ToInt(this["Id"]);
            }

            set
            {
                this["Id"] = value;
            }
        }

        [ConfigurationProperty("Name", IsRequired = false)]
        public string Name
        {
            get
            {
                return (string)this["Name"];
            }

            set
            {
                this["Name"] = value;
            }
        }

        [ConfigurationProperty("MinBettype", IsRequired = false)]
        public int MinBettype
        {
            get
            {
                return (int)this["MinBettype"];
            }

            set
            {
                this["MinBettype"] = value;
            }
        }

        [ConfigurationProperty("MaxBettype", IsRequired = false)]
        public int MaxBettype
        {
            get
            {
                return Converter.ToInt(this["MaxBettype"]);
            }

            set
            {
                this["MaxBettype"] = value;
            }
        }
    }
}