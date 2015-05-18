using System;
using System.Collections.Generic;
using System.IO;
using Fanex.Common.Extensions;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
using Fanex.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NegativeBalanceByMatch.Business.Entities;
using Fanex.Service.Core.Model;
using Fanex.Service.Core.Service.Implement.Npoi;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Match = Fanex.NegativeBalanceByMatch.Business.Entities.Match;

namespace Fanex.NegativeBalanceByMatch.Business.Business.Exports
{
    public class NegativeBalanceByMatchExcelService : INegativeBalanceByMatchExcelService
    {
        private NegativeBalanceByMatchService _negativeBalanceByMatchService;

        public NegativeBalanceByMatchExcelService(NegativeBalanceByMatchService negativeBalanceByMatchService)
        {
            _negativeBalanceByMatchService = negativeBalanceByMatchService;
        }

		//public void GenerateReportForLicensees()
		//{
		//    IDictionary<string, IList<Match>> licenseeToMatchs = _negativeBalanceByMatchService.GetLicenseeMatchsMap();
		//    //IDictionary<string, IList<Match>> licenseeToMatchs = GenLicenseeMatchs();

		//    ExportSettings exportSettings = new ExportSettings();

		//    licenseeToMatchs.ForEach(m => GenerateReportForLicensee(m.Key, m.Value, exportSettings));
		//}

        public void GenerateReportForLicensee(string licenseeName, IList<Match> matchs, ExportSettings exportSettings, string saveFileNamePath)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();

            NpoiNewFormatFontService fontService = new NpoiNewFormatFontService(workbook);
            NpoiNewFormatCustomCellStyleService cellStyleService = new NpoiNewFormatCustomCellStyleService(workbook);

            ExcelDocument excelDocument = new ExcelDocument(new XSSFWorkbook(), exportSettings, licenseeName);

            ExportCompositeCommand<IExportCommand> exportCommand = new ExportCompositeCommand<IExportCommand>(excelDocument);

            ExportMatchsCommand exportMatchsCommand = new ExportMatchsCommand(excelDocument, matchs, 3);
            foreach (var match in matchs)
            {
                ExportMatchInfoCommand matchInfoCommand = new ExportMatchInfoCommand(excelDocument, match.MatchInfo);
                ExportMatchSettleLogsCommand matchSettleLogsCommand = new ExportMatchSettleLogsCommand(excelDocument, match.MatchSettleLogList);
                ExportCustomerBalanceInfosCommand customerBalanceInfoCommand = new ExportCustomerBalanceInfosCommand(excelDocument, match.CustomerInfoList);

                ExportMatchCommand matchCommand = new ExportMatchCommand(excelDocument, matchInfoCommand, matchSettleLogsCommand, customerBalanceInfoCommand);

                exportMatchsCommand.Add(matchCommand);
            }

            IExportCommand autoSizeColumnCommand = new ExportAutoSizeColumCommand(excelDocument);

			IExportCommand saveCommand = new ExportSaveCommand(excelDocument, saveFileNamePath);

            exportCommand.Add(exportMatchsCommand);
            exportCommand.Add(autoSizeColumnCommand);
            exportCommand.Add(saveCommand);

            exportCommand.Execute();
        }

        public Match GenMatchData(int index)
        {
            MatchInfo matchInfo = new MatchInfo
            {
                AwayName = "Away" + index,
                HomeName = "Home" + index,
                EventDate = DateTime.Now,
                MatchCode = "1" + index,
                MatchID = "11111111" + index
            };

            List<MatchSettleLog> matchSettleLogs = new List<MatchSettleLog>();
            for (int i = 0; i < 10; i++)
            {
                MatchSettleLog matchSettleLog = new MatchSettleLog
                {
                    LogID = i,
                    SettlementTime = DateTime.Now,
                    Action = "Undo Mix Parlay Settlement" + i,
                    HTAwayScore = i + 1,
                    HTHomeScore = i + 2,
                    FinalAwayScore = i + 3,
                    FinalHomeScore = i + 4
                };

                matchSettleLogs.Add(matchSettleLog);
            }

            List<CustomerBalanceInfo> customerBalanceInfos = new List<CustomerBalanceInfo>();
            for (int i = 0; i < 4; i++)
            {
                CustomerBalanceInfo customerBalanceInfo = new CustomerBalanceInfo
                {
                    LogID = i,
                    Username = "Username" + i,
                    TotalBalance = -111 - i,
                    CurrencyName = "UUS",
                    Site = "Site" + i
                };

                customerBalanceInfos.Add(customerBalanceInfo);
            }

            Match match = new Match
            {
                MatchInfo = matchInfo,
                MatchSettleLogList = matchSettleLogs,
                CustomerInfoList = customerBalanceInfos
            };

            return match;
        }

        public IList<Match> GenListMatchs()
        {
            IList<Match> matches = new List<Match>();
            for (int i = 0; i < 3; i++)
            {
                Match match = GenMatchData(i);
                matches.Add(match);
            }

            return matches;
        }

        public IDictionary<string, IList<Match>> GenLicenseeMatchs()
        {
            IDictionary<string, IList<Match>> licenseeMatchs = new Dictionary<string, IList<Match>>();
            licenseeMatchs.Add("12BET", GenListMatchs());
            licenseeMatchs.Add("HAIFA", GenListMatchs());

            return licenseeMatchs;
        }
    }
}
