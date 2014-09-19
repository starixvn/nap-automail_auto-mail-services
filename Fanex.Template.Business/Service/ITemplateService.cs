// -----------------------------------------------------------------------
// <copyright file="IResetWinningService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Template.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Template.Business.Model;
    using Fanex.Template.Business.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITemplateService
    {
        void ProcessResetWinning();

        int InsertTask(SMAReportModel task);

        Status GetSatatusByTaskId(int Id);
    }
}
