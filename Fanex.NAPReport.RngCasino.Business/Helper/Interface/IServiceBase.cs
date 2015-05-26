// -----------------------------------------------------------------------
// <copyright file="IServiceBase.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Robot.Report.RngCasino.Helper.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IServiceBase
    {
        IEnumerable<T> GetReportData<T>(ICriteria criteria);
    }
}
