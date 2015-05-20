namespace Fanex.NAPReport.SMALoss.Business.Services.ExcelExport
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Extensions;

    public class ExportCompositeCommand<T> : ExportCommandBase where T : IExportCommand
    {
        private IList<T> _commands;

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
