namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
    public class ExportSettings
    {
        public ExportSettings()
        { 
            StartRowIndex = 1;
            StartColIndex = 1;
            MaxNumOfColumns = 5;
            ColumnPadding = 500;
        }

        public int StartRowIndex 
        { 
            get; set; 
        }

        public int StartColIndex
        {
            get;
            set;
        }

        public int MaxNumOfColumns
        {
            get;
            set;
        }

        public int ColumnPadding
        {
            get;
            set;
        }
    }
}
