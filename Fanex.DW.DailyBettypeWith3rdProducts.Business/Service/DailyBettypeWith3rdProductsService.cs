namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Databank.Core.Repositories.Implement.NpoiCellStyle;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Criteria;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Model;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Resource;
    using Fanex.Service.Core.Utilities;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// Daily BETTYPE With 3rd Products Service.
    /// </summary>
    public class DailyBettypeWith3rdProductsService : AutoMailBaseService
    {
        private DailyBettypeWith3rdProductsCriteria _criteria;

        private GetAllSportType _allSporttypeCriteria;

        private List<SportType> _sportType;

        private IDataReader _reader;

        private ISheet _sheet;

        private int _startCol = 1;

        private int _colSplit = 1;

        private int _rowSplit = 5;

        private int _currentRow = 1;

        private int _columnCount = 0;

        private string _dateFormat = "dd/MM/yyyy";

        private string _dateFileNameFormat = "yyyy-MM";

        private string _yyyyMMddFormat = "yyyy-MM-dd";

        private string _dateTimeFormat = "dd/MM/yyyy hh:mm:ss tt";

        private ExcelCellStyleRepository _cellStyle;

        private List<double> _totalRow;

        private bool _isVirtual;

        private string _exportDataDirectoryKey = "Fanex.DW.DailyBettypeWith3rdProducts.ExportDataDirectory";

        public DailyBettypeWith3rdProductsService()
        {
            this._criteria = new DailyBettypeWith3rdProductsCriteria();
            this._allSporttypeCriteria = new GetAllSportType();
            this._sportType = new List<SportType>();
            this._totalRow = new List<double>();
        }

        /// <summary>
        /// Generates the report.
        /// </summary>
        /// <param name="isVirtual">If set to <c>true</c> [is virtual].</param>
        public void GenerateReport(bool isVirtual)
        {
            this.Init();
            this._isVirtual = isVirtual;
            this.GenerateFileName();
            this.Workbook = new XSSFWorkbook();
            this._sheet = this.Workbook.CreateSheet();
            this._cellStyle = new ExcelCellStyleRepository(this.Workbook);
            this.GetAllSporttype();
            this.GenerateReportHeadline();
            this.GenerateAllSection();
            this.GenerateSummarySection();
            foreach (var sporttype in this._sportType)
            {
                this.GenerateSporttypeSection(sporttype);
            }

            string exportDataDirectory = AppSettings.ExportDataDirectory;
            this.GetFileName = exportDataDirectory + @"\" + this.FileName;
            SaveFile.SaveExcelFile(exportDataDirectory, this.FileName, this.Workbook);            
            
        }

        private void Init()
        { 
          this._startCol = 1;
          this._colSplit = 2;
          this._rowSplit = 5;
          this._currentRow = 1;
          this._columnCount = 0;
          this._sportType = new List<SportType>();
        }

        private void GenerateFileName()
        {
            this.FileName = string.Format(
                Label.DailyBettype3rdProductTitleFileName,
                DateTime.Today.ToString(this._yyyyMMddFormat),
                this._isVirtual ? Label.Virtual : string.Empty);
        }

        /// <summary>
        /// Gets all sport type.
        /// </summary>
        private void GetAllSporttype()
        {
            using (ObjectDb db = new ObjectDb(this._allSporttypeCriteria.GetSettingKey()))
            {
                db.AssignParamValues(this._allSporttypeCriteria.GetSpParams());
                var reader = db.ExecuteReader();
                while (reader.Read())
                {
                    this._sportType.Add(new SportType()
                    {
                        SportId = Converter.ToInt(reader[0]),
                        SportName = Converter.ToString2(reader[1])
                    });
                }
            }
        }

        /// <summary>
        /// Generates the report headline.
        /// </summary>
        private void GenerateReportHeadline()
        {
            this._sheet.CreateFreezePane(this._colSplit, this._rowSplit);
            this._sheet.DisplayGridlines = false;
            var row = this._sheet.CreateRow(this._currentRow++);

            var cell = row.CreateCell(this._startCol);
            cell.CellStyle = this._cellStyle.TitleStyle;
            cell.SetCellValue(string.Format(
                Label.ReportTitle,
                Label.DailyBettype3rdProductTitle,
                this._criteria.DateStart.ToString(this._dateFormat),
                this._criteria.DateEnd.ToString(this._dateFormat)));

            row = this._sheet.CreateRow(this._currentRow++);
            cell = row.CreateCell(this._startCol);
            cell.SetCellValue(string.Format(Label.GeneratedTitle, "it", DateTime.Now.ToString(this._dateTimeFormat)));
            cell.CellStyle = this._cellStyle.BoldStyle;
            this._currentRow++;
        }

        /// <summary>
        /// Generates all section.
        /// </summary>
        private void GenerateAllSection()
        {
            this._criteria = new DailyBettypeWith3rdProductsCriteria();
            this._criteria.PeriodId = 1;
            this._criteria.StatisticId = this._isVirtual ? 11 : 1;
            using (ObjectDb db = new ObjectDb(this._criteria.GetSettingKey()))
            {
                db.AssignParamValues(this._criteria.GetSpParams());
                this._reader = db.ExecuteReader();
                this.GenerateSection("ALL");
            }

            this._currentRow++;
        }

        /// <summary>
        /// Generates the summary section.
        /// </summary>
        private void GenerateSummarySection()
        {
            this._criteria = new DailyBettypeWith3rdProductsCriteria();
            this._criteria.StatisticId = this._isVirtual ? 11 : 1;
            using (ObjectDb db = new ObjectDb(this._criteria.GetSettingKey()))
            {
                db.AssignParamValues(this._criteria.GetSpParams());
                this._reader = db.ExecuteReader();
                this.GenerateSection("SUMMARY");
            }

            this._currentRow++;
        }

        /// <summary>
        /// Generates the sport type section.
        /// </summary>
        /// <param name="sporttype">The sport type.</param>
        private void GenerateSporttypeSection(SportType sporttype)
        {
            this._criteria = new DailyBettypeWith3rdProductsCriteria();
            this._criteria.PeriodId = 1;
            this._criteria.StatisticId = this._isVirtual ? 11 : 1;
            this._criteria.SportId = sporttype.SportId;
            using (ObjectDb db = new ObjectDb(this._criteria.GetSettingKey()))
            {
                db.AssignParamValues(this._criteria.GetSpParams());
                this._reader = db.ExecuteReader();
                if (this._reader.FieldCount != 0)
                {
                    this.GenerateSection(sporttype.SportName);
                    this._currentRow++;
                }
            }
        }

        /// <summary>
        /// Generates the section.
        /// </summary>
        /// <param name="title">The title.</param>
        private void GenerateSection(string title)
        {
            var row = this._sheet.CreateRow(this._currentRow++);
            var cell = row.CreateCell(1);
            cell.CellStyle = this._cellStyle.BoldStyle;
            cell.SetCellValue(title);
            this.GenerateSectionHeader();
            this.GenerateSectionBody();
            this.GenerateSectionFooter();
            this._sheet.SetColumnWidth(this._startCol, 4000);
            for (int i = 1; i < this._columnCount; i++)
            {
                this._sheet.AutoSizeColumn(i + this._startCol);
            }
        }

        /// <summary>
        /// Generates the section header.
        /// </summary>
        private void GenerateSectionHeader()
        {
            this._totalRow = new List<double>();
            this._columnCount = this._reader.FieldCount;
            var headerRow = this._sheet.CreateRow(this._currentRow++);
            for (int col = 0; col < this._columnCount; col++)
            {
                var colName = this._reader.GetName(col);
                bool isTotalColumn = colName.Contains("Total") || colName.Contains("total");
                var cell = headerRow.CreateCell(this._startCol + col);
                cell.CellStyle = this.GetHeaderCellStyle(this._cellStyle.HeaderStyles, col, this._columnCount, isTotalColumn);
                cell.SetCellValue(this._reader.GetName(col));
                this._totalRow.Add(0);
            }
        }

        /// <summary>
        /// Generates the section body.
        /// </summary>
        private void GenerateSectionBody()
        {
            while (this._reader.Read())
            {
                var row = this._sheet.CreateRow(this._currentRow++);
                for (int col = 0; col < this._columnCount; col++)
                {
                    var cell = row.CreateCell(this._startCol + col);
                    var colName = this._reader.GetName(col);
                    bool isTotalColumn = colName.Contains("Total") || colName.Contains("total");
                    var colType = this._reader.GetFieldType(col);
                    if (colType == typeof(DateTime))
                    {
                        cell.CellStyle = this.GetBodyCellStyle(this._cellStyle.BodyStyles.DateStyles, col, this._columnCount, isTotalColumn);
                        cell.SetCellValue(Converter.ToDateTime(this._reader[col]));
                    }
                    else if (colType == typeof(string))
                    {
                        cell.CellStyle = this.GetBodyCellStyle(this._cellStyle.BodyStyles.NormalStyles, col, this._columnCount, isTotalColumn);
                        cell.SetCellValue(Converter.ToString2(this._reader[col]));
                    }
                    else
                    {
                        double cellValue = (double)Converter.ToDecimal(this._reader[col]);
                        cell.CellStyle = this.GetBodyCellStyle(this._cellStyle.BodyStyles.IntegerStyles, col, this._columnCount, isTotalColumn);
                        cell.SetCellValue(cellValue);
                        this._totalRow[col] += cellValue;
                    }
                }
            }
        }

        /// <summary>
        /// Generates the section footer.
        /// </summary>
        private void GenerateSectionFooter()
        {
            var footerRow = this._sheet.CreateRow(this._currentRow++);
            for (int col = 0; col < this._columnCount; col++)
            {
                var colName = this._reader.GetName(col);
                bool isTotalColumn = colName.Contains("Total") || colName.Contains("total");
                var footerCell = footerRow.CreateCell(this._startCol + col);
                if (col == 0)
                {
                    footerCell.CellStyle = this._cellStyle.TotalStyles.NormalStyles.LeftStyle;
                    footerCell.SetCellValue("Total");
                }
                else
                {
                    footerCell.CellStyle = this.GetFooterCellStyle(this._cellStyle.TotalStyles.IntegerStyles, col, this._columnCount, isTotalColumn);
                    footerCell.SetCellValue(this._totalRow[col]);
                }
            }
        }
    }
}
