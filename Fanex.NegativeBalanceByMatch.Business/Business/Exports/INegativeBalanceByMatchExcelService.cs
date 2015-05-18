using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NegativeBalanceByMatch.Business.Entities;

namespace Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
    public interface INegativeBalanceByMatchExcelService
    {
        void GenerateReportForLicensees();
    }
}
