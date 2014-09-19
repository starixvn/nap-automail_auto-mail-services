// -----------------------------------------------------------------------
// <copyright file="Status.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Task is running.
        /// </summary>
        Running,

        /// <summary>
        /// Task is processing.
        /// </summary>
        Processing,

        /// <summary>
        /// The process reset winning is failed.
        /// </summary>
        ResetFailed,

        /// <summary>
        /// The process reset winning successful.
        /// </summary>
        ResetSuccessful,

        CannotFound
    }
}
