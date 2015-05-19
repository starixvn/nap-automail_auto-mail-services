namespace Fanex.NAPReport.SMALoss.Business.Business
{
    public interface ISMALossProcess
    {
        /// <summary>
        /// Notifies the email.
        /// </summary>
        /// <param name="templateUrl">The template URL.</param>
        /// <returns>Boolean value indicates whether the email is sent successfully or not.</returns>
        bool NotifyEmail(string templateUrl);
    }
}