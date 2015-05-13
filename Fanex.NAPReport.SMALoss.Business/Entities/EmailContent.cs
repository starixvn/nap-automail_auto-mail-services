namespace Fanex.NAPReport.SMALoss.Business.Entities
{
    /// <summary>
    /// Email content.
    /// </summary>
    public class EmailContent
    {
        public Recipients Recievers { get; set; }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}