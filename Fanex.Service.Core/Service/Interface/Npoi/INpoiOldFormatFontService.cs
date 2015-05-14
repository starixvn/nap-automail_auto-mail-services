// -----------------------------------------------------------------------
// <copyright file="INpoiOldFormatFontService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Interface.Npoi
{
    using Fanex.Service.Core.Model.NpoiOldFormat;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INpoiOldFormatFontService
    {
        IFont RegularFont { get; }

        IFont BlackBoldFont { get; }

        IFont WhiteBoldFont { get; }

        IFont CreateFont(FontInformation fontInformation);

        IFont CreateBoldFont(FontInformation fontInformation);
    }
}