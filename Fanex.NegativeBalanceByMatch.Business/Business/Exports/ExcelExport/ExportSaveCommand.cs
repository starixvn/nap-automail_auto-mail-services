using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;

namespace Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
	public class ExportSaveCommand : ExportCommandBase
	{
		private readonly string _saveFileNamePath;

		public ExportSaveCommand(ExcelDocument excelDocument, string saveFileNamePath)
			: base(excelDocument)
		{
			_saveFileNamePath = saveFileNamePath;
		}

		public override void Execute()
		{
			ExcelDocument.Save(_saveFileNamePath);
		}
	}
}
