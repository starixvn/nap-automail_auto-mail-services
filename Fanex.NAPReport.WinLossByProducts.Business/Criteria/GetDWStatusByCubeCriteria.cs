using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Data.Repository;

namespace Fanex.NAPReport.WinLossByProducts.Business.Criteria
{
    public class GetDWStatusByCubeCriteria : CriteriaBase
    {
        public string Cube
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "BI_GetDWStatusByCube";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
