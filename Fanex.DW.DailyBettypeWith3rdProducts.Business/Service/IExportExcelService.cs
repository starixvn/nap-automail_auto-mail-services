namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Service
{
    using System.Collections.Generic;

    public interface IExportExcelService
    {
        List<string> DirectoryOfFileNames { get; set; }

        List<string> FileNames { get; set; }

        void ExportExcelSeparateRealAndVirtual(bool isVirtual, bool isDaily);

        void ExportExcelCombineRealAndVirtual(bool isDaily);
    }
}
