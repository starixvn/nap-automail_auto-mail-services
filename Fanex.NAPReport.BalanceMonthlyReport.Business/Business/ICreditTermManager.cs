using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Xml;
using Fanex.NAPReport.BalanceMonthlyReport.Business.DataAccess;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Helper;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Infrastructure.Extensions;
using Fanex.Data;
using Fanex.NAPReport.BalanceMonthlyReport.Business;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Model;
using NPOI.SS.UserModel;
using Fanex.Service.Core.Model;
using Fanex.Service.Core.Service.Interface.Npoi;
using Fanex.Service.Core.Service.Implement.Npoi;
using Fanex.Service.Core.Extensions;

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Bussiness
{
    public interface ICreditTermManager
    {
        string ReportPath { get; }

        void ExportExcel(Report report);
    }
}
