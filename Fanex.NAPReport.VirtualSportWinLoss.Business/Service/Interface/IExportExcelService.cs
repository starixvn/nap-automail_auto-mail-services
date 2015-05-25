// WinLossSummary
// <copyright file="IExportExcelService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface
{
    using System.IO;

    /// <summary>
    /// Interfacing export excel for virtual sport's win loss.
    /// </summary>
    public interface IExportExcelService
    {
        bool ProcessDataToSendMail();
    }
}