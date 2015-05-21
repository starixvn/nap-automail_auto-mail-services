using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Common.Extensions;
using Fanex.NAPReport.Business.Business.Exports.ExcelExport;
using NPOI.XSSF.UserModel;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
    public class ExportCompositeCommand<T> : ExportCommandBase where T : IExportCommand
    {
        protected IList<T> _commands;

        public ExportCompositeCommand(ExcelDocument excelDocument)
            : base(excelDocument)
        {
            _commands = new List<T>();
        }

        public virtual void Add(T exportCommand)
        {
            _commands.Add(exportCommand);
        }

        public override void Execute()
        {
            _commands.ForEach(cmd => cmd.Execute());
        }
    }
}
