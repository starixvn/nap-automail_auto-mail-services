// -----------------------------------------------------------------------
// <copyright file="ExportExcelOandaExchangeRate.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.OandaExChangeRate.Testing.Service
{
    using System;
    using System.Collections;
    using System.Data;
    using System.IO;
    using Fanex.Data;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class ExportExcelOandaExchangeRate : ExportExcelOandaExchangeRateBase
    {
        public void GetDataByDate()
        {
            var baseCurrencies = AppSettings.BaseCurrencies;

            ExportHeader(baseCurrencies);

            ExportBody(baseCurrencies);

            SaveExcelFile();
        }

        private void ExportHeader(string[] baseCurrencies)
        {
            var row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ??
                      worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            _cellInfo.CellValue = "Currency";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);

            for (var i = 0; i < baseCurrencies.Length; i++)
            {
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                var columnName = baseCurrencies[i];
                _cellInfo.CellValue = columnName;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            }

            NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
        }

        private void ExportBody(string[] baseCurrencies)
        {
            using (var reader = GetData())
            {
                while (reader.Read())
                {
                    var row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ??
                              worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;

                    _cellInfo.CellValue = reader["Code"];
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);

                    for (var i = 0; i < baseCurrencies.Length; i++)
                    {
                        NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                        var columnName = baseCurrencies[i];
                        _cellInfo.CellValue = reader[columnName];
                        NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _decimalNumberStyle);
                    }

                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                    NPOIUtility.ResetColumnIndex(ref _cellInfo);
                }
            }
        }

        private IDataReader GetData()
        {
            IDataReader reader = null;
            var db = new ObjectDb("robert_testExchange");

            var param = new Hashtable();
            param.Add("date", "2015-04-01");
            db.AssignParamValues(param);

            reader = db.ExecuteReader();

            return reader;
        }

        private void SaveExcelFile()
        {
            var reportName = "OandaExchangeRate_01_" + DateTime.Now.ToShortFormatedDateWithSlash() + ".xlsx";
            reportName = reportName.Replace("/", "-");

            var fullpath = AppSettings.ExportDataDirectory + @"\" + reportName;
            if (File.Exists(fullpath))
            {
                reportName = reportName.Replace("OandaExchangeRate_01_", "OandaExchangeRate_02_");
            }

            SaveFile.SaveExcelFile(AppSettings.ExportDataDirectory, reportName, _workbook);

            GC.Collect();
        }
    }
}