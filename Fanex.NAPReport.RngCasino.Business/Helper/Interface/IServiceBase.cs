// -----------------------------------------------------------------------
// <copyright file="IServiceBase.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Fanex.NAP.Framework.Data;

namespace Fanex.NAPReport.RngCasino.Business.Helper.Interface
{
	/// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IServiceBase
    {
        IEnumerable<T> GetReportData<T>(ICriteria criteria);
    }
}
