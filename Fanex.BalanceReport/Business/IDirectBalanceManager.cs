using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Xml;
using Fanex.NAPReport.BalanceReport.DataAccess;
using Fanex.NAPReport.BalanceReport.Helper;
using Fanex.NAPReport.BalanceReport.Infrastructure.Extensions;
using Fanex.NAPReport.BalanceReport.Model;
using Fanex.NAPReport.BalanceReport.Core;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Model;

namespace Fanex.NAPReport.BalanceReport.Bussiness
{
    public interface IDirectBalanceManager
    {
        void ExportExcel();
    }
}
