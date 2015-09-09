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
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    
    public class BettypeWith3rdProductsExcelService
    {
        private DailyBettypeWith3rdProductsCriteria _criteria;
        private GetAllSportType _allSporttypeCriteria;
        private List<SportType> _sportType;
        private DataTable _dataTable;

        private XSSFSheet _workSheet;
        private XSSFWorkbook _workbook;

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
        private XSSFCellStyle _backgroundPinkBrightColorStyle;
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
        private Color _pinkBrightColor;
        private Color _greyColor;        
        private Color _orangeColor;
        private Color _orangeBrightColor;
        private Color _blueColor;
        private Color _blueBrightColor;

        public BettypeWith3rdProductsExcelService(XSSFWorkbook workbook)
        {
            _workbook = workbook;            

            this._criteria = new DailyBettypeWith3rdProductsCriteria();
            this._allSporttypeCriteria = new GetAllSportType();
            this._sportType = new List<SportType>();
            this._totalRow = new List<double>();                       
        }

        private void InitialFont()
        {
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _regularBoldFont = _fontService.RegularFont;
            _regularBoldFont.IsBold = true;
        }

        private void InitialColor()
        {
            _greenColor = Color.FromArgb(0, 255, 0);
            _pinkColor = Color.FromArgb(255, 0, 102);
            _pinkBrightColor = Color.FromArgb(218, 150, 148);
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
            _backgroundPinkBrightColorStyle = _customCellStyleService.CreateCenterTextStyle(_pinkBrightColor, _regularFont);
            _backgroundOrangeColorStyle = _customCellStyleService.CreateCenterTextStyle(_orangeColor, _regularFont);
            _backgroundOrangeBrightColorStyle = _customCellStyleService.CreateParenthesesNegativeIntegerNumberStyle(_orangeBrightColor, _regularFont);
            _backgroundBlueColorStyle = _customCellStyleService.CreateCenterTextStyle(_blueColor, _regularFont);

            _textRightStyle = _customCellStyleService.CreateRightTextStyle(Color.White, _regularFont);
            _textLeftStyle = _customCellStyleService.CreateLeftTextStyle(Color.White, _regularFont);
            _textTotalCenterStyle = _customCellStyleService.CreateCenterTextStyle(_greyColor, _regularFont);

            var backgroundInformation = new BackgroundInformation();
            var alignInfo = new AlignmentInformation();
            alignInfo.HorizontalAlignment = (int)HorizontalAlignment.Left;
            alignInfo.VerticalAlignment = (int)VerticalAlignment.Center;
            _textTitleLeftStyle = _customCellStyleService.CreateCellStyle(_borderInformation, backgroundInformation, alignInfo, _regularBoldFont);

            _numberStyle = _customCellStyleService.CreateParenthesesNegativeIntegerNumberStyle(Color.White, _regularFont);
            _numberTotalStyle = _customCellStyleService.CreateParenthesesNegativeIntegerNumberStyle(_greyColor, _regularFont);
            _numberTotalWLStyle = _customCellStyleService.CreateParenthesesNegativeIntegerNumberStyle(_blueBrightColor, _regularFont);
        }

        public void GenerateReport(bool isVirtual, bool isDaily)
        {
            this._isVirtual = isVirtual;
            this._isDaily = isDaily;
            this._startCol = 1;
            this._colSplit = 2;
            this._rowSplit = 5;
            this._currentRow = 1;
            this._columnCount = 0;
            this._sportType = new List<SportType>();
            this._dataTable = new DataTable();

            _workSheet = _workbook.CreateSheet(isVirtual ? "Virtual" : "Real") as XSSFSheet;

            this._cellStyle = new ExcelCellStyleRepository(this._workbook);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(this._workbook);

            short borderStyle = (short)BorderStyle.Thin;
            _borderInformation = new BorderInformation(borderStyle, borderStyle, borderStyle, borderStyle, Color.White, Color.White, Color.White, Color.White);

            InitialFont();
            InitialColor();
            InitialStyle();

            this.GetAllSporttype();
            this.GenerateReportHeadline();
            this.GenerateAllSection();
            this.GenerateSummarySection();
            foreach (var sporttype in this._sportType)
            {
                this.GenerateSporttypeSection(sporttype);
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
            this._workSheet.CreateFreezePane(this._colSplit, this._rowSplit);
            this._workSheet.DisplayGridlines = false;
            var row = this._workSheet.CreateRow(this._currentRow++);

            var cell = row.CreateCell(this._startCol);
            cell.CellStyle = this._cellStyle.TitleStyle;
            cell.SetCellValue(string.Format(
                Label.ReportTitle,
                Label.DailyBettype3rdProductTitle,
                this._criteria.DateStart.ToString(this._dateFormat),
                this._criteria.DateEnd.ToString(this._dateFormat)));

            row = this._workSheet.CreateRow(this._currentRow++);
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

                this.GenerateSection(Resource.ALL, false, 0);
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

                this.GenerateSection(Resource.SUMMARY, false, 0);
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
                    this.GenerateSection(sporttype.SportName, true, sporttype.SportId);
                    this._currentRow++;
                }
            }
        }

        private void GenerateSection(string title, bool isSporttype, int sportId)
        {
            GenerateTitle(title);

            this._columnCount = this._dataTable.Columns.Count;
            if (!isSporttype)
            {
                _beginColumn = 0;
                _endColumn = _columnCount;

                GenerateSubSection(title, isSporttype, 0, sportId);
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

                    if (_endColumn > _beginColumn)
                    {
                        GenerateSubSection(title, isSporttype, indexSportSection, sportId);
                    }
                }
            }

            this._workSheet.SetColumnWidth(this._startCol, 4000);
            for (int i = 1; i < this._columnCount; i++)
            {
                this._workSheet.AutoSizeColumn(i + this._startCol, true);
            }
        }

        private void GenerateSubSection(string title, bool isSporttype, int indexSportSection, int sportId)
        {
            this.GenerateSectionHeader(title, isSporttype, indexSportSection, sportId);
            this.GenerateSectionBody(isSporttype, indexSportSection);
            this.GenerateSectionFooter(isSporttype, indexSportSection);
        }

        private void GenerateTitle(string title)
        {
            var row = this._workSheet.CreateRow(this._currentRow++);
            var cell = row.CreateCell(1);
            cell.CellStyle = _textTitleLeftStyle;
            cell.SetCellValue(title);
        }

        private void GenerateSectionHeader(string title, bool isSporttype, int indexSportSection, int sportId)
        {
            this._totalRow = new List<double>();
            var headerRow = this._workSheet.CreateRow(_currentRow++);
            IRow headerRow1 = isSporttype ? this._workSheet.CreateRow(_currentRow++) : null;
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
                SetHeaderBackGroundColor(title, cell, sportId);
                cell.SetCellValue(colName);

                if (isSporttype)
                {
                    var cell1 = headerRow1.CreateCell(this._startCol + columnIndex);
                    SetHeaderBackGroundColor(title, cell1, sportId);
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
                XSSFRow currentRow = _workSheet.GetRow(rowIndex) as XSSFRow ?? _workSheet.CreateRow(rowIndex) as XSSFRow;

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

            _workSheet.AddMergedRegion(new CellRangeAddress(firstRow, lastRow, firstColumn, lastColumn));            
        }

        private void SetHeaderBackGroundColor(string title, ICell cell, int sportId)
        {
            if (title.Equals(Resource.ALL))
            {
                _backgroundStyle = _backgroundGreenColorStyle;
            }
            else if (title.Equals(Resource.SUMMARY))
            {
                _backgroundStyle = _backgroundPinkColorStyle;
            }
            else if (title.ToLower().Equals("soccer"))
            {
                _backgroundStyle = _backgroundOrangeColorStyle;
            }
            else
            {
                if (sportId >= 2 && sportId <= 99)
                {
                    _backgroundStyle = _backgroundBlueColorStyle;
                }
                else
                {
                    _backgroundStyle = _backgroundPinkBrightColorStyle;
                }
            }

            cell.CellStyle = _backgroundStyle;
        }

        private void GenerateSectionBody(bool isSporttype, int indexSportSection)
        {
            string date = string.Empty;
            foreach (DataRow dataRow in _dataTable.Rows)
            {            
                var row = this._workSheet.CreateRow(this._currentRow++);
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
            var footerRow = this._workSheet.CreateRow(this._currentRow++);            
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