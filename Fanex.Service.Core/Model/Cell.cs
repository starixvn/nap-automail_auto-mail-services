// -----------------------------------------------------------------------
// <copyright file="Cell.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Cell
    {
        private int _currentColumn;

        private int _currentRow;

        private int _endColumn;

        private int _endRow;

        private object _cellValue;

        public Cell()
        {
            this._currentColumn = 0;
            this._endColumn = 0;
            this._currentRow = 0;
            this._endRow = 0;
            this._cellValue = 0;
        }

        public Cell(int currentColumn, int currentRow, int endColumn, int endRow, object cellValue)
        {
            this._currentColumn = currentColumn;
            this._endColumn = endColumn;
            this._currentRow = currentRow;
            this._endRow = endRow;
            this._cellValue = cellValue;
        }

        public int CurrentColumn
        {
            get
            {
                return this._currentColumn;
            }

            set
            {
                this._currentColumn = value;
            }
        }

        public int EndColumn
        {
            get
            {
                return this._endColumn;
            }

            set
            {
                this._endColumn = value;
            }
        }

        public int CurrentRow
        {
            get
            {
                return this._currentRow;
            }

            set
            {
                this._currentRow = value;
            }
        }

        public int EndRow
        {
            get
            {
                return this._endRow;
            }

            set
            {
                this._endRow = value;
            }
        }

        public object CellValue
        {
            get
            {
                return this._cellValue;
            }

            set
            {
                this._cellValue = value;
            }
        }

        public void IncreaseColumnIndex()
        {
            this._currentColumn = this._endColumn + 1;
            this._endColumn = this._endColumn + 1;
        }

        public void IncreaseRowIndex()
        {
            this._currentRow = this._endRow + 1;
            this._endRow = this._endRow + 1;
        }

        public void ResetColumnIndex()
        {
            this._currentColumn = 0;
            this._endColumn = 0;
        }
    }
}
