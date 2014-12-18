using System;
using Fanex.Logging;

namespace Fanex.NegativeBalanceByMatch.Business.Helper.Implement
{
    public class LogHandler
    {
        public static void Log(Exception ex)
        {
            LogService.Error(ex.Message, ex);
            FileExtension.WriteFile(ex.Message + "\n" + ex.StackTrace + "\n");
            

        }

        public static void Log(string message = "", LogType logType = LogType.Error)
        {
            switch (logType)
            {
                case LogType.Info:
                    LogService.Info(message);
                    break;
                case LogType.Error:
                    LogService.Error(message);
                    break;
            }
        }
    }

    public enum LogType : int
    {
        Info = 1,
        Error = 2
    }
}
