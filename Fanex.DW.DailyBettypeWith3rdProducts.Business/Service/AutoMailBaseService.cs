namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Service
{
    using Fanex.Databank.Core.Repositories.Implement.NpoiCellStyle;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Auto Mail Base Service.
    /// </summary>
    public class AutoMailBaseService
    {
        protected string FileName { get; set; }

        protected IWorkbook Workbook { get; set; }

        public string GetFileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the header cell style.
        /// </summary>
        /// <param name="headerCellStyle">The header cell style.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="columnCount">The column count.</param>
        /// <param name="isTotalColumn">If set to <c>true</c> [is total column].</param>
        /// <returns>Cell Style.</returns>
        protected ICellStyle GetHeaderCellStyle(HeaderCellStyleRepository headerCellStyle, int columnIndex, int columnCount, bool isTotalColumn = false)
        {
            if (columnIndex == 0)
            {
                return isTotalColumn ? headerCellStyle.LeftTotalColumnStyle : headerCellStyle.LeftStyle;
            }
            else if (columnIndex == columnCount - 1)
            {
                return isTotalColumn ? headerCellStyle.RightTotalColumnStyle : headerCellStyle.RightStyle;
            }
            else
            {
                return isTotalColumn ? headerCellStyle.TotalColumnStyle : headerCellStyle.Style;
            }
        }

        /// <summary>
        /// Gets the body cell style.
        /// </summary>
        /// <param name="bodyCellStyle">The body cell style.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="columnCount">The column count.</param>
        /// <param name="isTotalColumn">If set to <c>true</c> [is total column].</param>
        /// <returns>Cell Style.</returns>
        protected ICellStyle GetBodyCellStyle(BodyCellStyleRepository bodyCellStyle, int columnIndex, int columnCount, bool isTotalColumn = false)
        {
            if (columnIndex == 0)
            {
                return isTotalColumn ? bodyCellStyle.LeftTotalColumnStyle : bodyCellStyle.LeftStyle;
            }
            else if (columnIndex == columnCount - 1)
            {
                return isTotalColumn ? bodyCellStyle.RightTotalColumnStyle : bodyCellStyle.RightStyle;
            }
            else
            {
                return isTotalColumn ? bodyCellStyle.TotalColumnStyle : bodyCellStyle.Style;
            }
        }

        /// <summary>
        /// Gets the footer cell style.
        /// </summary>
        /// <param name="footerCellStyle">The footer cell style.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="columnCount">The column count.</param>
        /// <param name="isTotalColumn">If set to <c>true</c> [is total column].</param>
        /// <returns>Cell Style.</returns>
        protected ICellStyle GetFooterCellStyle(FooterCellStyleRepository footerCellStyle, int columnIndex, int columnCount, bool isTotalColumn = false)
        {
            if (columnIndex == 0)
            {
                return isTotalColumn ? footerCellStyle.LeftTotalColumnStyle : footerCellStyle.LeftStyle;
            }
            else if (columnIndex == columnCount - 1)
            {
                return isTotalColumn ? footerCellStyle.RightTotalColumnStyle : footerCellStyle.RightStyle;
            }
            else
            {
                return isTotalColumn ? footerCellStyle.TotalColumnStyle : footerCellStyle.Style;
            }
        }        
    }
}
