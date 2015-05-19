namespace Fanex.HorseTransaction3rdServices.Libs
{
    using System;
    using Fanex.HorseTransaction3rdServices.Service;

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
