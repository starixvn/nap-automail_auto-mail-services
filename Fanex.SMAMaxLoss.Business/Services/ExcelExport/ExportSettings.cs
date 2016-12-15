namespace Fanex.NAPReport.SMALoss.Business.Services.ExcelExport
{
    public class ExportSettings
    {
        public int StartRowIndex
        {
            get
            {
                return 1;
            }
        }

        public int StartColIndex
        {
            get
            {
                return 1;
            }
        }

        public int MaxNumOfColumns
        {
            get
            {
                return 8;
            }
        }

        public int ColumnPadding
        {
            get
            {
                return 500;
            }
        }
    }
}
