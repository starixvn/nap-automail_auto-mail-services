namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
    using System.Collections.Generic;
    using Fanex.Common.Extensions;
    using Fanex.NAPReport.Business.Business.Exports.ExcelExport;

    public class ExportCompositeCommand<T> : ExportCommandBase where T : IExportCommand
    {
        public ExportCompositeCommand(ExcelDocument excelDocument)
            : base(excelDocument)
        {
            _commands = new List<T>();
        }

        public IList<T> _commands { get; set; }

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
