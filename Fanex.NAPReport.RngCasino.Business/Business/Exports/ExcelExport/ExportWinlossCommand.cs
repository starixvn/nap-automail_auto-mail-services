namespace Fanex.NAPReport.RngCasino.Business.Business.Exports.ExcelExport
{
    using System.Collections.Generic;
    using System.Drawing;
    using Fanex.NAPReport.RngCasino.Business.Entities;
    using Fanex.NAPReport.RngCasino.Business.Helper.Implement;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public class ExportWinLossCommand : ExportCommandBase
    {
        private readonly IList<UserInfo> _userInfos;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private XSSFFont _regularFont;

        private XSSFCellStyle _headerStyle;
        private XSSFCellStyle _decimalStyle;
        private XSSFCellStyle _textCenterStyle;
        private XSSFCellStyle _textRightStyle;

        public ExportWinLossCommand(ExcelDocument excelDocument, IList<UserInfo> userInfos)
            : base(excelDocument)
        {
            _userInfos = userInfos;

            _fontService = new NpoiNewFormatFontService(excelDocument.Workbook);
            _regularFont = _fontService.RegularFont;
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(excelDocument.Workbook);

            _headerStyle = _customCellStyleService.CreateCenterTextStyle(Color.FromArgb(60, 94, 165), _fontService.WhiteBoldFont);
            _decimalStyle = _customCellStyleService.CreateDecimalNumberStyle(Color.White, _regularFont);
            _textCenterStyle = _customCellStyleService.CreateCenterTextStyle(Color.White, _regularFont);
            _textRightStyle = _customCellStyleService.CreateRightTextStyle(Color.White, _regularFont);
        }

        public override void Execute()
        {
            RenderWinLossHeader();
            RenderWinLossBody();
        }

        private void RenderWinLossHeader()
        {
            ExcelDocument.RenderCurrentCell("Index", _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Member", _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Agency", _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(string.Format("Winloss Amount in {0}", AppSettings.BaseCurrency), _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Winloss Amount in user currency (UserCurrency)", _headerStyle);
        }

        private void RenderWinLossBody()
        {
            for (int index = 0; index < _userInfos.Count; index++)
            {
                var userInfo = _userInfos[index];
                ExcelDocument.IncreaseRowIndex();
                ExcelDocument.ResetColumnIndex();

                RenderWinLossRow(index, userInfo);
            }
        }

        private void RenderWinLossRow(int index, UserInfo userInfo)
        {
            var styleCenter = ExcelDocument.GetCellStyle(FontBoldWeight.None, borderColor: Color.FromArgb(0, 0, 0));
            var styleRight = ExcelDocument.GetCellStyle(FontBoldWeight.None, borderColor: Color.FromArgb(0, 0, 0), horizontalAlignment: HorizontalAlignment.Right);

            ExcelDocument.RenderCurrentCell(index + 1, _textCenterStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(userInfo.Member, _textCenterStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(userInfo.Agency, _textCenterStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(userInfo.WinlossBaseCurrency, _decimalStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(string.Format("{0} ({1})", NumberExtension.WinLossFormat(userInfo.Winloss, 2), userInfo.CurrencyName), _textRightStyle);
        }
    }
}