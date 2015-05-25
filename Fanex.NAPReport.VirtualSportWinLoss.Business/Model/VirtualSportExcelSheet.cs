// -----------------------------------------------------------------------
// <copyright file="VirtualSportExcelSheet.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Franklin.Nguyen" date="2014.05.30">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Model
{
    using System.Collections.Generic;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VirtualSportExcelSheet
    {
        public string SheetName { get; set; }

        public List<WinLossDetailBySportType> SportType { get; set; }
    }
}