// -----------------------------------------------------------------------
// <copyright file="MailInfo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Model
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailInfo
    {
        private bool _isDaily = false;

        public MailInfo(bool IsDaily)
        {
            _isDaily = IsDaily;
        }

        public string DailyBettype3rdProductMailBody
        {
            get
            {
                if (_isDaily)
                {
                    return "This is the Bettype with 3rd products Daily update. Please see attached files for details.";
                }
                else
                {
                    return "This is the Bettype with 3rd products Weekly update. Please see attached files for details.";
                }
            }
        }

        public string ExportDataDirectory
        {
            get
            {
                if (_isDaily)
                {
                    return AppSettings.ExportDataDirectoryDaily;
                }
                else
                {
                    return AppSettings.ExportDataDirectoryWeekly;
                }
            }
        }
    }
}
