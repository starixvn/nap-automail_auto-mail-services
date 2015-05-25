// -----------------------------------------------------------------------
// <copyright file="Status.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Enums
{
    /// <summary>
    /// Email send status.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Task is running.
        /// </summary>
        Running,

        /// <summary>
        /// The email was sent successful.
        /// </summary>
        SentEmailSuccessful,

        /// <summary>
        /// The email was sent failure.
        /// </summary>
        SentEmailFailure
    }
}