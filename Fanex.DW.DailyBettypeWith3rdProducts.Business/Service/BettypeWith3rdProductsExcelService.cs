namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Databank.Core.Repositories.Implement.NpoiCellStyle;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Criteria;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Model;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Resource;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Model.NpoiBase;
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    
    public class BettypeWith3rdProductsExcelService
    {
        private DailyBettypeWith3rdProductsCriteria _criteria;
        private GetAllSportType _allSporttypeCriteria;
        private List<SportType> _sportType;
        private DataTable _dataTable;
        private XSSFSheet _sheet;
        private int _startCol = 1;
        private int _colSplit = 1;
        private int _rowSplit = 5;
        private int _currentRow = 1;
        private int _columnCount = 0;
        private string _dateFormat = "dd/MM/yyyy";
        private string _dateTimeFormat = "dd/MM/yyyy hh:mm:ss tt";
        private ExcelCellStyleRepository _cellStyle;
        private List<double> _totalRow;
        private bool _isVirtual;
        private bool _isDaily;
        private int _numOfColumnsOnEachSection = 70;
        private int _numOfSections = 0;
        private int _beginColumn = 0;
        private int _endColumn = 0;

        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private INpoiNewFormatFontService _fontService;
        private BorderInformation _borderInformation;

        private XSSFFont _regularFont;
        private XSSFFont _regularBoldFont;

        private XSSFCellStyle _backgroundGreenColorStyle;
        private XSSFCellStyle _backgroundPinkColorStyle;
        private XSSFCellStyle _backgroundOrangeColorStyle;
        private XSSFCellStyle _backgroundOrangeBrightColorStyle;
        private XSSFCellStyle _backgroundBlueColorStyle;
        
        private XSSFCellStyle _backgroundStyle;
        private XSSFCellStyle _textRightStyle;
        private XSSFCellStyle _textLeftStyle;
        private XSSFCellStyle _textTotalCenterStyle;
        private XSSFCellStyle _textTitleLeftStyle;
        private XSSFCellStyle _numberStyle;
        private XSSFCellStyle _numberTotalStyle;
        private XSSFCellStyle _numberTotalWLStyle;

        private Color _greenColor;
        private Color _pinkColor;
        private Color _greyColor;        
        private Color _orangeColor;
        private Color _orangeBrightColor;
        private Color _blueColor;
        private Color _blueBrightColor;

        public BettypeWith3rdProductsExcelService()
        {
            this._criteria = new DailyBettypeWith3rdProductsCriteria();
            this._allSporttypeCriteria = new GetAllSportType();
            this._sportType = new List<SportType>();
            this._totalRow = new List<double>();                       
        }

        public string FileName { get; set; }

        private XSSFWorkbook Workbook { get; set; }

        public string GetFileName
        {
            get;
            set;
        }

        private void InitialSettings()
        {
            _borderInformation = new BorderInformation(0, 0, 0, 0, Color.White, Color.White, Color.White, Color.White);
        }

        private void InitialFont()
        {
            _fontService = new NpoiNewFormatFontService(Workbook);
            _regularFont = _fontService.RegularFont;
            _regularBoldFont = _fontService.RegularFont;
            _regularBoldFont.IsBold = true;
        }

        private void InitialColor()
        {
            _greenColor = Color.FromArgb(0, 255, 0);
            _pinkColor = Color.FromArgb(255, 0, 102);
            _greyColor = Color.FromArgb(217, 217, 217);
            _blueBrightColor = Color.FromArgb(204, 255, 255);
            _blueColor = Color.FromArgb(51, 204, 204);
            _orangeColor = Color.FromArgb(226, 107, 10);
            _orangeBrightColor = Color.FromArgb(253, 233, 217);
        }

        private void InitialStyle()
        {
            _backgroundGreenColorStyle = _customCellStyleService.CreateCenterTextStyle(_greenColor, _regularFont);
            _backgroundPinkColorStyle = _customCellStyleService.CreateCenterTextStyle(_pinkColor, _regularFont);
            _backgroundOrangeColorStyle = _customCellStyleService.CreateCenterTextStyle(_orangeColor, _regularFont);
            _backgroundOrangeBrightColorStyle = _customCellStyleService.CreateParenthesesNegativeIntegerNumberStyle(_orangeBrightColor, _regularFont);
            _backgroundBlueColorStyle = _customCellStyleService.CreateCenterTextStyle(_blueColor, _regularFont);

            _textRightStyle = _customCellStyleService.CreateRightTextStyle(Color.White, _regularFont);
            _textLeftStyle = _customCellStyleService.CreateLeftTextStyle(Color.White, _regularFont);
            _textTotalCenterStyle = _customCellStyleService.CreateCenterTextStyle(_greyColor, _regularFont);
            _textTitleLeftStyle = _customCellStyleService.CreateCellStyle(_borderInformation, new BackgroundInformation(), new AlignmentInformation(), _regularBoldFont);

            _numberStyle = _customCellStyleService.CreateParenthesesNegativeIntegerNumberStyle(Color.White, _regularFont);
            _numberTotalStyle = _customCellStyleService.CreateParenthesesNegativeIntegerNumberStyle(_greyColor, _regularFont);
            _numberTotalWLStyle = _customCellStyleService.CreateParenthesesNegativeIntegerNumberStyle(_blueBrightColor, _regularFont);
        }

        public void GenerateReport(bool isVirtual, bool isDaily, string exportDataDirectory)
        {
            this.Init();
            this._isVirtual = isVirtual;
            this._isDaily = isDaily;
            this.GenerateFileName();
            this.GetAllSporttype();
            this.GenerateReportHeadline();
            this.GenerateAllSection();
            this.GenerateSummarySection();
            foreach (var sporttype in this._sportType)
            {
                this.GenerateSporttypeSection(sporttype);
            }

            this.GetFileName = exportDataDirectory + @"\" + this.FileName;
            SaveFile.SaveExcelFile(exportDataDirectory, this.FileName, this.Workbook, AppSettings.ExcelPassword);
        }

        private void Init()
        {
            this.Workbook = new XSSFWorkbook();
            this._sheet = Workbook.CreateSheet("Sheet1") as XSSFSheet;
            this._cellStyle = new ExcelCellStyleRepository(this.Workbook);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(this.Workbook);

            InitialSettings();
            InitialFont();
            InitialColor();
            InitialStyle();

            this._startCol = 1;
            this._colSplit = 2;
            this._rowSplit = 5;
            this._currentRow = 1;
            this._columnCount = 0;
            this._sportType = new List<SportType>();
            this._dataTable = new DataTable();
        }

        private void GenerateFileName()
        {
            if (this._isDaily)
            {
                if (this._isVirtual)
                {
                    this.FileName = string.Format(Resource.DailyFileNameVirtual, DateTime.Today.ToString("yyyy_MM_dd"));
                }
                else
                {
                    this.FileName = string.Format(Resource.DailyFileNameReal, DateTime.Today.ToString("yyyy_MM_dd"));
                }
            }
            else
            {
                if (this._isVirtual)
                {
                    this.FileName = string.Format(Resource.WeeklyFileNameVirtual, DateTime.Today.ToString("yyyy_MM_dd"));
                }
                else
                {
                    this.FileName = string.Format(Resource.WeeklyFileNameReal, DateTime.Today.ToString("yyyy_MM_dd"));
                }
            }
        }

        private void GetAllSporttype()
        {
            using (ObjectDb db = new ObjectDb(this._allSporttypeCriteria.GetSettingKey()))
            {
                this._allSporttypeCriteria.Include3rd = !_isVirtual;

                db.AssignParamValues(this._allSporttypeCriteria.GetSpParams());
                var reader = db.ExecuteReader();

                int count = 0;
                while (reader.Read())
                {
                    this._sportType.Add(new SportType()
                    {
                        SportId = Converter.ToInt(reader[0]),
                        SportName = Converter.ToString2(reader[1])
                    });

                    if (AppSettings.IsSingleSport)
                    {
                        if (count == 0)
                        {
                            break;
                        }
                    }

                    count++;
                }
            }
        }

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

        private void GenerateAllSection()
        {
            this._criteria = new DailyBettypeWith3rdProductsCriteria();
            this._criteria.PeriodId = 1;
            this._criteria.StatisticId = this._isVirtual ? 11 : 1;
            using (ObjectDb db = new ObjectDb(this._criteria.GetSettingKey()))
            {
                db.AssignParamValues(this._criteria.GetSpParams());
                var dataSet = db.ExecuteDataSet();
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    this._dataTable = dataSet.Tables[0];
                }
                else
                {
                    this._dataTable = new DataTable();
                }

                this.GenerateSection(Resource.ALL, false);
            }

            this._currentRow++;
        }

        private void GenerateSummarySection()
        {
            this._criteria = new DailyBettypeWith3rdProductsCriteria();
            this._criteria.StatisticId = this._isVirtual ? 11 : 1;            
            using (ObjectDb db = new ObjectDb(this._criteria.GetSettingKey()))
            {
                db.AssignParamValues(this._criteria.GetSpParams());
                var dataSet = db.ExecuteDataSet();
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    _dataTable = dataSet.Tables[0];
                }
                else
                {
                    this._dataTable = new DataTable();
                }

                this.GenerateSection(Resource.SUMMARY, false);
            }

            this._currentRow++;
        }

        private void GenerateSporttypeSection(SportType sporttype)
        {
            this._criteria = new DailyBettypeWith3rdProductsCriteria();
            this._criteria.PeriodId = 1;
            this._criteria.StatisticId = this._isVirtual ? 11 : 1;
            this._criteria.SportId = sporttype.SportId;
            using (ObjectDb db = new ObjectDb(this._criteria.GetSettingKey()))
            {
                db.AssignParamValues(this._criteria.GetSpParams());

                var dataSet = db.ExecuteDataSet();
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    _dataTable = dataSet.Tables[0];
                }
                else
                {
                    this._dataTable = new DataTable();
                }

                if (this._dataTable.Columns.Count != 0)
                {
                    this.GenerateSection(sporttype.SportName, true);
                    this._currentRow++;
                }
            }
        }

        private void GenerateSection(string title, bool isSporttype)
        {
            GenerateTitle(title);

            this._columnCount = this._dataTable.Columns.Count;
            if (!isSporttype)
            {
                _beginColumn = 0;
                _endColumn = _columnCount;

                GenerateSubSection(title, isSporttype, 0);
            }
            else
            {
                _numOfSections = _columnCount / _numOfColumnsOnEachSection;
                if (_columnCount % _numOfColumnsOnEachSection > 0)
                {
                    _numOfSections++;
                }

                for (int indexSportSection = 0; indexSportSection < _numOfSections; indexSportSection++)
                {
                    if (indexSportSection == 0)
                    {
                        _beginColumn = 0;
                        _endColumn = _numOfColumnsOnEachSection + 1;
                    }
                    else
                    {
                        _beginColumn = _endColumn;
                        _endColumn = _beginColumn + _numOfColumnsOnEachSection;
                    }

                    if (_endColumn > _columnCount)
                    {
                        _endColumn = _columnCount;
                    }

                    if (indexSportSection > 0 && _endColumn > _beginColumn)
                    {
                        this._currentRow++;
                        GenerateTitle(title);
                    }

                    GenerateSubSection(title, isSporttype, indexSportSection);
                }
            }

            this._sheet.SetColumnWidth(this._startCol, 4000);
            for (int i = 1; i < this._columnCount; i++)
            {
                this._sheet.AutoSizeColumn(i + this._startCol, true);
            }
        }

        private void GenerateSubSection(string title, bool isSporttype, int indexSportSection)
        {
            this.GenerateSectionHeader(title, isSporttype, indexSportSection);
            this.GenerateSectionBody(isSporttype, indexSportSection);
            this.GenerateSectionFooter(isSporttype, indexSportSection);
        }

        private void GenerateTitle(string title)
        {
            var row = this._sheet.CreateRow(this._currentRow++);
            var cell = row.CreateCell(1);
            cell.CellStyle = _textTitleLeftStyle;
            cell.SetCellValue(title);
        }

        private void GenerateSectionHeader(string title, bool isSporttype, int indexSportSection)
        {
            this._totalRow = new List<double>();
            var headerRow = this._sheet.CreateRow(_currentRow++);
            IRow headerRow1 = isSporttype ? this._sheet.CreateRow(_currentRow++) : null;
            int columnIndex = 0;
            int numBettype = 0;

            if (isSporttype && indexSportSection > 0 && _endColumn > _beginColumn)
            {
                RenderMergeCell(_backgroundStyle, "Date", _currentRow - 2, _currentRow - 1, this._startCol + columnIndex, this._startCol + columnIndex);
                
                columnIndex++;
                this._totalRow.Add(0);
            }

            for (int col = _beginColumn; col < _endColumn; col++)
            {
                var colName = _dataTable.Columns[col].ColumnName;
                colName = colName.Contains("Column") ? string.Empty : colName;

                var cell = headerRow.CreateCell(this._startCol + columnIndex);
                SetHeaderBackGroundColor(title, cell);
                cell.SetCellValue(colName);

                if (isSporttype)
                {
                    var cell1 = headerRow1.CreateCell(this._startCol + columnIndex);
                    SetHeaderBackGroundColor(title, cell1);
                    cell1.SetCellValue(colName);

                    var arrColNames = colName.Split(new[] { "@#@" }, StringSplitOptions.RemoveEmptyEntries);
                    if (arrColNames.Length == 2)
                    {
                        numBettype++;

                        cell.SetCellValue(arrColNames[0]);
                        cell1.SetCellValue(arrColNames[1]);

                        if (numBettype == 1)
                        {
                            RenderMergeCell(_backgroundStyle, arrColNames[0], _currentRow - 2, _currentRow - 2, this._startCol + columnIndex, this._startCol + columnIndex + numBettype);
                            if (col == _endColumn - 1 && indexSportSection < _numOfSections)
                            {
                                _endColumn++;
                            }
                        }
                        else if (numBettype == 2)
                        {
                            numBettype = 0;
                        }
                    }
                    else
                    {
                        RenderMergeCell(_backgroundStyle, colName, _currentRow - 2, _currentRow - 1, this._startCol + columnIndex, this._startCol + columnIndex);
                        numBettype = 0;
                    }
                }               

                this._totalRow.Add(0);

                columnIndex++;
            }            
        }

        private void RenderMergeCell(XSSFCellStyle cellStyle, object cellValue, int firstRow, int lastRow, int firstColumn, int lastColumn)
        {
            for (var rowIndex = firstRow; rowIndex <= lastRow; rowIndex++)
            {
                XSSFRow currentRow = _sheet.GetRow(rowIndex) as XSSFRow ?? _sheet.CreateRow(rowIndex) as XSSFRow;

                for (var cellIndex = firstColumn; cellIndex <= lastColumn; cellIndex++)
                {
                    XSSFCell currentCell = currentRow.GetCell(cellIndex) as XSSFCell ?? currentRow.CreateCell(cellIndex) as XSSFCell;
                    if (rowIndex == firstRow && cellIndex == firstColumn)
                    {
                        NPOIUtility.SetCellValue(currentCell, cellValue);
                    }

                    if (rowIndex == _currentRow ||
                        rowIndex == lastRow ||
                        cellIndex == firstColumn ||
                        cellIndex == lastColumn)
                    {
                        currentCell.CellStyle = cellStyle;
                    }                    
                }
            }

            _sheet.AddMergedRegion(new CellRangeAddress(firstRow, lastRow, firstColumn, lastColumn));            
        }

        private void SetHeaderBackGroundColor(string title, ICell cell)
        {
            if (title.Equals(Resource.ALL))
            {
                cell.CellStyle = _backgroundGreenColorStyle;
                _backgroundStyle = _backgroundGreenColorStyle;
            }
            else if (title.Equals(Resource.SUMMARY))
            {
                cell.CellStyle = _backgroundPinkColorStyle;
                _backgroundStyle = _backgroundPinkColorStyle;
            }
            else if (title.ToLower().Equals("soccer"))
            {
                cell.CellStyle = _backgroundOrangeColorStyle;
                _backgroundStyle = _backgroundOrangeColorStyle;
            }
            else
            {
                cell.CellStyle = _backgroundBlueColorStyle;
                _backgroundStyle = _backgroundBlueColorStyle;
            }
        }

        private void GenerateSectionBody(bool isSporttype, int indexSportSection)
        {
            string date = string.Empty;
            foreach (DataRow dataRow in _dataTable.Rows)
            {
                var row = this._sheet.CreateRow(this._currentRow++);
                int columnIndex = 0;
                int numBettype = 0;

                if (isSporttype && indexSportSection > 0 && _endColumn > _beginColumn)
                {
                    var dateCell = row.CreateCell(this._startCol + columnIndex);
                    dateCell.CellStyle = _textRightStyle;
                    dateCell.SetCellValue(Converter.ToDateTime(dataRow[0]).ToString("dd MMM yyyy"));

                    columnIndex++;
                }

                for (int col = _beginColumn; col < _endColumn; col++)
                {
                    var cell = row.CreateCell(this._startCol + columnIndex);
                    var colName = _dataTable.Columns[col].ColumnName;
                    bool isTotalColumn = colName.Contains("Total WL") || colName.Contains("total wl");
                    var colType = dataRow[col].GetType();
                    var arrColNames = colName.Split(new[] { "@#@" }, StringSplitOptions.RemoveEmptyEntries);

                    if (colType == typeof(DateTime))
                    {
                        date = Converter.ToDateTime(dataRow[col]).ToString("dd MMM yyyy");
                        cell.CellStyle = _textRightStyle;
                        cell.SetCellValue(date);
                    }
                    else if (colType == typeof(string))
                    {
                        cell.CellStyle = _textLeftStyle;
                        cell.SetCellValue(Converter.ToString2(dataRow[col]));
                    }                    
                    else
                    {
                        double cellValue = (double)Converter.ToDecimal(dataRow[col]);
                        if (isTotalColumn)
                        {
                            cell.CellStyle = _numberTotalWLStyle;
                        }
                        else
                        {
                            cell.CellStyle = _numberStyle;
                        }

                        cell.SetCellValue(cellValue);
                        this._totalRow[columnIndex] += cellValue;

                        if (arrColNames.Length == 2)
                        {
                            numBettype++;
                            if (numBettype == 1)
                            {
                                cell.CellStyle = _backgroundOrangeBrightColorStyle;
                            }
                            else if (numBettype == 2)
                            {
                                numBettype = 0;
                            }
                        }
                        else
                        {
                            numBettype = 0;
                        }
                    }

                    columnIndex++;
                }
            }
        }

        private void GenerateSectionFooter(bool isSporttype, int indexSportSection)
        {
            var footerRow = this._sheet.CreateRow(this._currentRow++);            
            int columnIndex = 0;

            if (isSporttype && indexSportSection > 0 && _endColumn > _beginColumn)
            {
                var totalCell = footerRow.CreateCell(this._startCol + columnIndex);
                totalCell.CellStyle = _textTotalCenterStyle;
                totalCell.SetCellValue("Total");

                columnIndex++;
            }

            for (int col = _beginColumn; col < _endColumn; col++)
            {
                var colName = _dataTable.Columns[col].ColumnName;
                bool isTotalColumn = colName.Contains("Total WL") || colName.Contains("total wl");
                var footerCell = footerRow.CreateCell(this._startCol + columnIndex);
                if (columnIndex == 0)
                {
                    footerCell.CellStyle = _textTotalCenterStyle;
                    footerCell.SetCellValue("Total");
                }
                else
                {
                    if (isTotalColumn)
                    {
                        footerCell.CellStyle = _numberTotalWLStyle;
                    }
                    else
                    {
                        footerCell.CellStyle = _numberTotalStyle;
                    }

                    footerCell.SetCellValue(this._totalRow[columnIndex]);
                }

                columnIndex++;
            }
        }
    }
}