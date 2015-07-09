// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="" date="2014.04.21">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business
{
    using System;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// Application setting.
    /// </summary>
    public class AppSettings : AppSettingBase, ISettings
    {
        public static string DataDirectory
        {
            get;
            set;
        }

        public static string DataDirectoryWithYearMonth
        {
            get { return ExportDataDirectoryWithYearMonth(DataDirectory); }
        }

        public static string ExcelPasswordProtected
        {
            get;
            set;
        }

        public static int FunctionId
        {
            get;
            set;
        }

        public static bool IsTest
        { 
            get; 
            set; 
        }

        public static DateTime DateStart
        {
            get;
            set;
        }

        public static DateTime DateEnd
        {
            get;
            set;
        }
    }
}