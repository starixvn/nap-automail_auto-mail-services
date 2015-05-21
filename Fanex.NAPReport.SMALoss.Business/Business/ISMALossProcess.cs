namespace Fanex.NAPReport.SMALoss.Business.Business
{
    public interface ISMALossProcess
    {
        /// <summary>
        /// Notifies the email.
        /// </summary>
        /// <returns>Boolean value indicates whether the email is sent successfully or not.</returns>
        bool NotifyEmail();
    }
}