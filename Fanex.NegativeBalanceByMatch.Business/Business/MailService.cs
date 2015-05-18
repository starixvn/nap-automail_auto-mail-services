using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;

namespace Fanex.NegativeBalanceByMatch.Business.Business
{
    public class MailService
    {
	    private INegativeBalanceByMatchExcelService _negativeBalanceByMatchExcelService ;

		public MailService(INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService)
		{
			_negativeBalanceByMatchExcelService = negativeBalanceByMatchExcelService;
		}

    }
}
