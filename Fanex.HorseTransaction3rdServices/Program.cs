namespace Fanex.NAP.HorseTransaction3rd.Business.Libs
{
    using System;
    using Fanex.NAP.HorseTransaction3rd.Business.Service;

    public class Program
    {
        public static void Main(string[] args)
        {
            var ftp = new GetFilesTransactionManager();
            ftp.GetFilesTransaction();

            Console.WriteLine("waiting for 3s ...");
            System.Threading.Thread.Sleep(3000);
        }
    }
}
