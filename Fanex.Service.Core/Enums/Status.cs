// -----------------------------------------------------------------------
// <copyright file="Status.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Enums
{
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
        /// The process is failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The process successful.
        /// </summary>
        Successful,

        /// <summary>
        /// The can not found.
        /// </summary>
        CannotFound,

        /// <summary>
        /// Data does not enough.
        /// </summary>
        NotEnoughDataToSendEmail
    }
}