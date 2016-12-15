namespace Fanex.NAPReport.SMALoss.Business.Entities
{
    using Fanex.MailDelivery.Client.Configurations;

    /// <summary>
    /// Email content.
    /// </summary>
    public class EmailContent
    {
        public int MailingListId
        {
            get
            {
                return AppSettings.GetFunctionId("Kaden Testing");
            }
        }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}