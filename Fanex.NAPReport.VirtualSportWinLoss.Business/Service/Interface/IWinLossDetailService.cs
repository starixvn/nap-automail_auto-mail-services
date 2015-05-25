// <copyright file="IWinLossDetailService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail;

    /// <summary>
    /// Win loss summary of virtual sport service's interface.
    /// </summary>
    public interface IWinLossDetailService
    {
        List<WinLossDetailBySportType> GetWinLossDetail();
    }
}