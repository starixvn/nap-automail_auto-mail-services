using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Service
{
    public interface IExportExcelService
    {
        List<string> DirectoryOfFileNames { get; set; }

        List<string> FileNames { get; set; }

        void ExportExcelSeparateRealAndVirtual(bool isVirtual, bool isDaily);

        void ExportExcelCombineRealAndVirtual(bool isDaily);
    }
}
