// -----------------------------------------------------------------------
// <copyright file="INoinNewFormatFontService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Interface.Npoi
{
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INpoiNewFormatFontService
    {
        XSSFFont RegularFont { get; }

        XSSFFont BlackBoldFont { get; }

        XSSFFont WhiteBoldFont { get; }

        XSSFFont CreateFont(FontInformation fontInformation);

        XSSFFont CreateBoldFont(FontInformation fontInformation);
    }
}