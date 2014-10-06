// -----------------------------------------------------------------------
// <copyright file="CellInfo.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public struct CellInfo
    {
        public int CurrentColumn;

        public int CurrentRow;

        public int EndColumn;

        public int EndRow;

        public object CellValue;

        public CellInfo(int currentColumn, int currentRow, int endColumn, int endRow, object cellValue)
        {
            this.CurrentColumn = currentColumn;
            this.EndColumn = endColumn;
            this.CurrentRow = currentRow;
            this.EndRow = endRow;
            this.CellValue = cellValue;
        }
    }
}
