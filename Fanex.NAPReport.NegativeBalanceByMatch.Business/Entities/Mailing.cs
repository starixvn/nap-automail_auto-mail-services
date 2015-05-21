using System.Data;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities
{
    public class Mailing
    {
        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string LicenseeName { get; set; }

        public bool IsValid
        {
            get { return (this.LicenseeName.Equals(string.Empty) && this.To.Equals(string.Empty) ? false : true); }
          
        } 
        public Mailing()
        {
            this.LicenseeName = string.Empty;
            this.To = string.Empty;
            this.Cc = string.Empty;
            this.Bcc = string.Empty;
           
        }
    }
}
