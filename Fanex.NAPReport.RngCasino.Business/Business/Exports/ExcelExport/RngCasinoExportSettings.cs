namespace Fanex.NAPReport.RngCasino.Business.Business.Exports.ExcelExport
{
    public class RngCasinoExportSettings
    {
        public RngCasinoExportSettings()
        {
            StartRowIndex = 1;
            StartColIndex = 1;
            ColumnPadding = 500;
            MaxNumOfColumns = 5;
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

        public int ColumnPadding
        {
            get;
            set;
        }

        public int MaxNumOfColumns
        {
            get;
            set;
        }
    }
}
