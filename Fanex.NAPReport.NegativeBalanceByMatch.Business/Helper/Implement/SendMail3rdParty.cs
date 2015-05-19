using System.Net.Mail;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper.Implement
{
    public class SendMail3rdParty
    {
        public static bool SendMail(string subject, string title, string mailContent, string filePathAttach, string tomail, string ccMail, string bccMail, string frommail, string pwd)
        {
            MailMessage msg = new MailMessage(); //create the message


            tomail = tomail.Replace(";", ",");
            ccMail = ccMail.Replace(";", ",");
            bccMail = bccMail.Replace(";", ",");

            msg.To.Add(tomail);
            if (ccMail.Length > 0)
                msg.CC.Add(ccMail);
            if (bccMail.Length > 0)
                msg.Bcc.Add(bccMail);

            msg.From = new MailAddress(frommail, title, System.Text.Encoding.UTF8);
            msg.Subject = subject;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;

            msg.IsBodyHtml = true;
            msg.Body = mailContent;
            msg.BodyEncoding = System.Text.Encoding.UTF8;

            if (filePathAttach.Length > 0)
            {
                Attachment attachFile = new Attachment(filePathAttach);
                msg.Attachments.Add(attachFile);
            }

            msg.Priority = MailPriority.High;

            //Network Credentials for Gmail
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(frommail, pwd);
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            try
            {
                client.Send(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
