namespace Fanex.NAPReport.HorseTransaction3rd.Testing
{
    using System;
    using System.IO;
    using System.Linq;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Resource;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Service;

    public class Program
    {
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            Console.WriteLine("Running Horse Transaction 3rd");

            var ftp = new GetFilesTransactionManager();
            ftp.GetFilesTransaction();

            var content = string.Join("<br/>", ftp.MailList.Select(i => i.Value));

            var mail = new Mail()
            {
                Subject = Resource.SubjectOfMail,
                Title = Resource.SubjectOfMail,
                Body = content
            };
            Console.WriteLine("Done at " + DateTime.Now.ToString());
        }
    }
}
