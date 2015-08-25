namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Model
{
    public class MailInfo
    {
        private bool _isDaily = false;

        public MailInfo(bool isDaily)
        {
            _isDaily = isDaily;
        }

        public string ExportDataDirectory
        {
            get
            {
                if (_isDaily)
                {
                    return AppSettings.DirectoryForDaily;
                }
                else
                {
                    return AppSettings.DirectoryForWeekly;
                }
            }
        }
    }
}
