// -----------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.RngCasino.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRepository
    {
        void UpdateLastTime();

        DateTime GetLastTime();
    }
}
